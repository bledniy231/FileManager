using System.Net;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using PianoMentor.BLL.Services.MultipartRequestHelper;
using PianoMentor.Contract.Models.DataSet;
using PianoMentor.DAL;
using PianoMentor.DAL.Models.DataSet;
using BinaryData = PianoMentor.DAL.Models.DataSet.BinaryData;

namespace PianoMentor.BLL.Services.FilesUploadManagers;

public class UploadFilesManager(
	IMultipartRequestHelper multipartRequestHelper, 
	FormOptions formOptions,
	IServiceProvider serviceProvider) : IUploadFilesManager
{
	private readonly Dictionary<FileExtensionsCollectionsEnum, string[]> _allowedExtensions = new()
	{
		{ FileExtensionsCollectionsEnum.AllExcludeExecutable, [".exe", ".bat", ".msi", ".com", ".cmd"] },
		{ FileExtensionsCollectionsEnum.Images, [".webp", ".png", ".jpg", ".jpeg", ".jfif"] },
		{ FileExtensionsCollectionsEnum.Documents, [".pdf"] }
	};
	
	private static readonly byte[] PngSignature = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];
	private static readonly byte[] JpegSignature = [0xFF, 0xD8];
	private static readonly byte[] WebPSignature = [0x52, 0x49, 0x46, 0x46, 0x57, 0x45, 0x42, 0x50];
	private static readonly byte[] JfifSignature = [0xFF, 0xD8, 0xFF, 0xE0, 0x4A, 0x46, 0x49, 0x46, 0x00];
	private static readonly byte[][] ImageSignatures =
	[
		PngSignature,
		JpegSignature,
		WebPSignature,
		JfifSignature
	];

	private static readonly byte[] PdfSignature = [0x25, 0x50, 0x44, 0x46];
	private static readonly byte[][] DocumentsSignatures = 
	[
		PdfSignature
	];

	private bool _isOneFileUploadAllowed = false;
	private FileExtensionsCollectionsEnum[] _extensionsOption = [FileExtensionsCollectionsEnum.AllExcludeExecutable];
	
	public void AllowOnlyOneFileUpload(bool isAllowed = true)
		=> _isOneFileUploadAllowed = isAllowed;
	
	public void AddCheckingFileExtensions(params FileExtensionsCollectionsEnum[] option)
		=> _extensionsOption = option;
	
	public async Task<UploadingResponse> UploadAsync(long ownerId, BinaryTypeEnum binaryType, string contentType, Stream requestBody, CancellationToken cancellationToken)
	{
		using var scope = serviceProvider.CreateScope();
		using var dbContext = scope.ServiceProvider.GetRequiredService<PianoMentorDbContext>();
		
		if (!multipartRequestHelper.IsMultipartContentType(contentType))
		{
			return new UploadingResponse(null, ["Unsupported media type in request"]);
		}

		var managedUser = await dbContext.Users
			.Include(u => u.DataSets)
			.FirstOrDefaultAsync(u => u.Id == ownerId, cancellationToken);
		if (managedUser == null)
		{
			return new UploadingResponse(null, ["Incorrect userId"]);
		}

		var storage = dbContext.Storages.FirstOrDefault(s => s.AllowWrite);
		if (storage == null)
		{
			return new UploadingResponse(null, ["No storage with write access"]);
		}

		var createdAt = DateTime.UtcNow;
		var newDataSet = new DataSet
		{
			IsDraft = true,
			CreatedAt = createdAt,
			Storage = storage,
			OwnerId = ownerId
		};

		dbContext.Add(newDataSet);
		dbContext.SaveChanges();
		newDataSet = await dbContext.DataSets
			.FirstOrDefaultAsync(ds => 
				!ds.IsDeleted 
				&& ds.IsDraft 
				&& ds.CreatedAt == createdAt 
				&& ds.OwnerId == ownerId,
				cancellationToken);

		var binaries = new List<BinaryData>();

		var boundary = multipartRequestHelper.GetBoundary(
			MediaTypeHeaderValue.Parse(contentType),
			formOptions.MultipartBoundaryLengthLimit);
		if (boundary == null)
		{
			return new UploadingResponse(null, ["Cannot get boundary from request"]);
		}

		var reader = new MultipartReader(boundary, requestBody);
		var section = await reader.ReadNextSectionAsync(cancellationToken);
		int fileIndex = 0;
		int totalReadBytes = 0;
		List<string> failedLoadFilesWithErrors = [];

		while (section != null)
		{
			if (fileIndex > 0 && _isOneFileUploadAllowed)
			{
				failedLoadFilesWithErrors.Add("Only one file can be uploaded according settings");
				break;
			}

			fileIndex++;

			if (ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition))
			{
				if (!multipartRequestHelper.HasFileContentDisposition(contentDisposition))
				{
					failedLoadFilesWithErrors.Add($"File index: {fileIndex}, " +
					                               $"Error: Wrong file content disposition, it should be like that: " +
					                               $"Content-Disposition: form-data; name=\"myfile1\"; filename=\"Misc 002.jpg\"");
				}

				string? trustedFileNameForDisplay = WebUtility.HtmlEncode(contentDisposition.FileName.Value);
				if (trustedFileNameForDisplay == null)
				{
					failedLoadFilesWithErrors.Add($"File index: {fileIndex}; Cannot encode file name from Content Disposition");
					continue;
				}

				string? fileExtension = GetFileExtension(trustedFileNameForDisplay);
				if (fileExtension == null)
				{
					failedLoadFilesWithErrors.Add($"File name: {trustedFileNameForDisplay}; Error: Wrong file extension or no file extension at all");
					continue;
				}

				string uniqueFileName =
					Path.GetFileNameWithoutExtension(trustedFileNameForDisplay)
					+ '_'
					+ Guid.NewGuid().ToString()
					+ fileExtension;

				var binary = new BinaryData
				{
					DataId = binaries.Count,
					Filename = uniqueFileName,
					DataSet = newDataSet,
					DataSetId = newDataSet.Id,
					BinaryTypeId = (int)binaryType
				};

				try
				{
					var file = binary.GetInternalFile();
					if (!file.Directory.Exists)
					{
						file.Directory.Create();
					}

					byte[] buffer = new byte[2 * 1024 * 1024];
					int readBytes = 0;

					using var targetStream = File.Create(file.FullName);
					bool isFirstReadingOfBytes = true;
					while ((readBytes = section.Body.ReadAtLeast(buffer, 2 * 1024 * 1024, false)) > 0)
					{
						if (isFirstReadingOfBytes)
						{
							isFirstReadingOfBytes = false;
							if (!IsPureFileCorrect(buffer))
							{
								failedLoadFilesWithErrors.Add($"File name: {file.Name}; Error: File have incorrect extension");
								break;
							}
						}
						targetStream.Write(buffer, 0, readBytes);
						totalReadBytes += readBytes;
					}

					totalReadBytes = 0;
					binary.Length = targetStream.Length;
					binaries.Add(binary);
				}
				catch (Exception ex)
				{
					failedLoadFilesWithErrors.Add($"File name: {trustedFileNameForDisplay}, Exception: {ex.Message};");
				}
			}
			else
			{
				failedLoadFilesWithErrors.Add($"File index: {fileIndex}, Error: Cannot parse \"Content disposition header value\"");
			}

			section = await reader.ReadNextSectionAsync(cancellationToken);
		}
		
		if (failedLoadFilesWithErrors.Count > 0)
		{
			Directory.Delete(newDataSet.GetDataSetDirectory(), true);
			dbContext.DataSets.Remove(newDataSet);
			dbContext.SaveChanges();
			failedLoadFilesWithErrors.Add("Some files failed to load, rollback uploading files... Try again");
			return new UploadingResponse(null, [..failedLoadFilesWithErrors]);
		}
		
		if (binaryType == BinaryTypeEnum.UserProfilePhoto)
		{
			var user = await dbContext.Users
				.Include(u => u.DataSets)
				.ThenInclude(ds => ds.Binaries)
				.Where(u => u.Id == ownerId)
				.FirstOrDefaultAsync(cancellationToken);

			var photos = user?.DataSets
				.Where(ds => !ds.IsDraft)
				.SelectMany(ds => ds.Binaries)
				.Where(b => 
					!b.IsDeleted 
					&& b.BinaryTypeId == (int)BinaryTypeEnum.UserProfilePhoto)
				.ToList();
			
			foreach (var ph in photos ?? [])
			{
				ph.IsDeleted = true;
			}
		}

		newDataSet.Binaries = binaries;
		newDataSet.IsDraft = false;
		managedUser.DataSets.Add(newDataSet);
		await dbContext.SaveChangesAsync(cancellationToken);
		
		return new UploadingResponse(newDataSet);
	}
    
    private string? GetFileExtension(string fileName)
    {
    	string? originalExtension = Path.GetExtension(fileName)?.ToLowerInvariant();
	    if (originalExtension == null 
	        || _allowedExtensions[FileExtensionsCollectionsEnum.AllExcludeExecutable]
		        .Contains(originalExtension))
	    {
		    return null;
	    }

	    if (_extensionsOption.Contains(FileExtensionsCollectionsEnum.AllExcludeExecutable))
	    {
		    return originalExtension;
	    }

	    return _extensionsOption.Any(opt => _allowedExtensions[opt].Contains(originalExtension)) 
		    ? originalExtension 
		    : null;
    }

    private static UploadingResponse TerminateUploading(PianoMentorDbContext dbContext, DataSet dataSet, string[] errors)
    {
    	Directory.Delete(dataSet.GetDataSetDirectory(), true);
    	dbContext.DataSets.Remove(dataSet);
    	dbContext.SaveChanges();
    	return new UploadingResponse(null, errors);
    }

    private bool IsPureFileCorrect(byte[] buffer)
    {
	    return _extensionsOption.Any(opt =>
	    {
			return opt switch
		    {
			    FileExtensionsCollectionsEnum.Images => ImageSignatures.Any(s => StartsWithSignature(buffer, s)),
			    FileExtensionsCollectionsEnum.Documents => DocumentsSignatures.Any(s => StartsWithSignature(buffer, s)),
			    _ => true
		    };
	    });
    }

    private static bool StartsWithSignature(byte[] buffer, byte[] signature)
    {
    	if (buffer.Length < signature.Length)
    	{
    		return false;
    	}

	    return !signature.Where((t, i) => buffer[i] != t).Any();
    }
}