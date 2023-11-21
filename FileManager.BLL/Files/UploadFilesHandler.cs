using FileManager.BLL.MultipartRequestHelper;
using FileManager.BLL.UploadPercentageChecker;
using FileManager.Contract.Default;
using FileManager.Contract.Files;
using FileManager.DAL;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace FileManager.BLL.Files
{
	internal class UploadFilesHandler(
		FileManagerDbContext dbContext,
		FormOptions formOptions,
		IMultipartRequestHelper multipartRequestHelper,
		IPercentageChecker percentageChecker) 
		: IRequestHandler<UploadFilesRequest, DefaultResponse>
	{
		private readonly IMultipartRequestHelper _multipartRequestHelper = multipartRequestHelper;
		private readonly IPercentageChecker _percentageChecker = percentageChecker;
		private readonly FileManagerDbContext _dbContext = dbContext;
		private readonly FormOptions _formOptions = formOptions;
		private readonly List<string> _failedLoadFilesWithErrors = [];

		public async Task<DefaultResponse> Handle(UploadFilesRequest request, CancellationToken cancellationToken)
		{
			var managedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
			if (managedUser == null)
			{
				return new DefaultResponse(false, ["Incorrect userId"]);
			}

			var storage = await _dbContext.Storages.FirstOrDefaultAsync(s => s.AllowWrite, cancellationToken);
			if (storage == null)
			{
				return new DefaultResponse(false, ["No storage with write access"]);
			}

			var dataSet = new DAL.Domain.DataSet.DataSet
			{
				CreatedAt = DateTime.UtcNow,
				IsDraft = true,
				Owner = managedUser,
				Storage = storage
			};
			_dbContext.DataSets.Add(dataSet);
			await _dbContext.SaveChangesAsync(cancellationToken);

			var binaries = new List<DAL.Domain.DataSet.BinaryData>();

			if (!_multipartRequestHelper.IsMultipartContentType(request.ContentType))
			{
				return new DefaultResponse(false, ["Unsupported media type"]);
			}

			var boundary = _multipartRequestHelper.GetBoundary(
				MediaTypeHeaderValue.Parse(request.ContentType), 
				_formOptions.MultipartBoundaryLengthLimit);

			var reader = new MultipartReader(boundary, request.Body);
			var section = await reader.ReadNextSectionAsync(cancellationToken);
			int fileIndex = 0;
			int totalReadBytes = 0;

			while (section != null)
			{
				fileIndex++;

				if (ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition))
				{
					if (!_multipartRequestHelper.HasFileContentDisposition(contentDisposition))
					{
						_failedLoadFilesWithErrors.Add($"File index: {fileIndex}, " +
							$"Error: Wrong file content disposition, it should be like that: " +
							$"Content-Disposition: form-data; name=\"myfile1\"; filename=\"Misc 002.jpg\"");
					}

					var trustedFileNameForDisplay = WebUtility.HtmlEncode(contentDisposition.FileName.Value);
					string uniqueFileName = Guid.NewGuid().ToString() + "_" + trustedFileNameForDisplay;

					var binary = new DAL.Domain.DataSet.BinaryData
					{
						DataId = binaries.Count,
						Filename = uniqueFileName,
						DataSet = dataSet,
						DataSetId = dataSet.Id
					};

					try
					{
						var file = binary.GetInternalFile();
						if (!file.Directory.Exists)
						{
							file.Directory.Create();
						}

						byte[] buffer = new byte[16 * 1024];
						int readBytes = 0;

						using var targetStream = File.Create(file.FullName);

						while ((readBytes = await section.Body.ReadAsync(buffer, cancellationToken)) > 0)
						{
							await targetStream.WriteAsync(buffer.AsMemory(0, readBytes), cancellationToken);
							totalReadBytes += readBytes;
							_percentageChecker.SetPercentage(request.UserId, (float)(totalReadBytes / section.Body.Length * 100.0));
						}

						totalReadBytes = 0;
						_percentageChecker.IncreaseFilesCountAlreadyUploaded(request.UserId);
						binary.Length = targetStream.Length;
						binaries.Add(binary);
					}
					catch (Exception ex) 
					{
						_failedLoadFilesWithErrors.Add($"File index: {fileIndex}, Exception: {ex.Message};");
					}
				}
				else
				{
					_failedLoadFilesWithErrors.Add($"File index: {fileIndex}, Error: Cannot parse \"Content disposition header value\"");
				}

				section = await reader.ReadNextSectionAsync(cancellationToken);
			}

			if (fileIndex == _failedLoadFilesWithErrors.Count)
			{
				_dbContext.DataSets.Remove(dataSet);
				await _dbContext.SaveChangesAsync();
				Directory.Delete(dataSet.GetDataSetDirectory());
				_failedLoadFilesWithErrors.Add("All files failed to load");
				return new DefaultResponse(false, [.. _failedLoadFilesWithErrors]);
			}

			dataSet.Binaries = binaries;
			dataSet.IsDraft = false;

			managedUser.DataSets.Add(dataSet);
			await _dbContext.SaveChangesAsync(cancellationToken);

			if (_failedLoadFilesWithErrors.Count != 0)
			{
				_failedLoadFilesWithErrors.Add("Some files failed to load");
				return new DefaultResponse(false, [.. _failedLoadFilesWithErrors]);
			}

			_percentageChecker.RemoveElement(request.UserId);

			return new DefaultResponse(true, null);
		}
	}
}
