using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using PianoMentor.BLL.MultipartRequestHelper;
using PianoMentor.Contract.Default;
using PianoMentor.Contract.Quizzes;
using PianoMentor.DAL;
using PianoMentor.DAL.Domain.DataSet;
using System.Net;

namespace PianoMentor.BLL.Quizzes
{
	internal class UploadQuestionImageHandler(
		PianoMentorDbContext dbContext,
		FormOptions formOptions,
		IMultipartRequestHelper multipartRequestHelper) 
		: IRequestHandler<UploadQuestionImageRequest, DefaultResponse>
	{
		private readonly IMultipartRequestHelper _multipartRequestHelper = multipartRequestHelper;
		private readonly PianoMentorDbContext _dbContext = dbContext;
		private readonly FormOptions _formOptions = formOptions;
		private readonly string[] _fileExtensions = [".webp", ".png", ".jpg", ".jpeg", ".jfif"];
		private readonly static byte[] PNGSignature = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];
		private readonly static byte[] JPEGSignature = [0xFF, 0xD8];
		private readonly static byte[] WebPSignature = [0x52, 0x49, 0x46, 0x46, 0x57, 0x45, 0x42, 0x50];
		private readonly static byte[] JFIFSignature = [0xFF, 0xD8, 0xFF, 0xE0, 0x4A, 0x46, 0x49, 0x46, 0x00];

		public async Task<DefaultResponse> Handle(UploadQuestionImageRequest request, CancellationToken cancellationToken)
		{
			if (!_multipartRequestHelper.IsMultipartContentType(request.ContentType))
			{
				return new DefaultResponse(["Unsupported media type"]);
			}

			var managedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
			if (managedUser == null)
			{
				return new DefaultResponse(["Incorrect userId"]);
			}

			var storage = _dbContext.Storages.FirstOrDefault(s => s.AllowWrite);
			if (storage == null)
			{
				return new DefaultResponse(["No storage with write access"]);
			}

			var questionDb = _dbContext.QuizQuestions.FirstOrDefault(q => q.QuestionId == request.QuestionId && !q.IsDeleted);
			if (questionDb == null)
			{
				return new DefaultResponse([$"No question with {request.QuestionId} found in DB"]);
			}

			questionDb.AttachedDataSet = new DAL.Domain.DataSet.DataSet
			{
				IsDraft = true,
				CreatedAt = DateTime.UtcNow,
				Storage = storage,
				OwnerId = request.UserId
			};
			_dbContext.SaveChanges();

			var binaries = new List<DAL.Domain.DataSet.BinaryData>();

			var boundary = _multipartRequestHelper.GetBoundary(
				MediaTypeHeaderValue.Parse(request.ContentType),
				_formOptions.MultipartBoundaryLengthLimit);

			var reader = new MultipartReader(boundary, request.Body);
			var section = await reader.ReadNextSectionAsync(cancellationToken);
			int fileIndex = 0;
			int totalReadBytes = 0;

			while (section != null)
			{
				if (fileIndex > 0)
				{
					return TerminateUploading(questionDb.AttachedDataSet, ["Only one file can be attached to the course item"]);
				}

				fileIndex++;

				if (ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition))
				{
					if (!_multipartRequestHelper.HasFileContentDisposition(contentDisposition))
					{
						return TerminateUploading(questionDb.AttachedDataSet, [$"File index: {fileIndex}, " +
							$"Error: Wrong file content disposition, it should be like that: " +
							$"Content-Disposition: form-data; name=\"myfile1\"; filename=\"Misc 002.jpg\""]);
					}

					string? trustedFileNameForDisplay = WebUtility.HtmlEncode(contentDisposition.FileName.Value);
					if (trustedFileNameForDisplay == null)
					{
						return TerminateUploading(questionDb.AttachedDataSet, ["Cannot encode file name from Content Disposition"]);
					}

					string? fileExtension = GetFileExtension(trustedFileNameForDisplay);
					if (fileExtension == null)
					{
						return TerminateUploading(questionDb.AttachedDataSet, [$"File name: {trustedFileNameForDisplay}, Error: Wrong file extension or totaly no file extension"]);
					}

					string uniqueFileName =
						Path.GetFileNameWithoutExtension(trustedFileNameForDisplay)
						+ '_'
						+ Guid.NewGuid().ToString()
						+ fileExtension;

					var binary = new DAL.Domain.DataSet.BinaryData
					{
						DataId = binaries.Count,
						Filename = uniqueFileName,
						DataSet = questionDb.AttachedDataSet,
						DataSetId = questionDb.AttachedDataSet.Id
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
								if (IsImage(buffer))
								{
									return TerminateUploading(questionDb.AttachedDataSet, [$"File name: {file.Name}, Error: File is not an image"]);
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
						return TerminateUploading(questionDb.AttachedDataSet, [$"File name: {trustedFileNameForDisplay}, Exception: {ex.Message};"]);
					}
				}
				else
				{
					return TerminateUploading(questionDb.AttachedDataSet, [$"File index: {fileIndex}, Error: Cannot parse \"Content disposition header value\""]);
				}

				section = await reader.ReadNextSectionAsync(cancellationToken);
			}

			questionDb.AttachedDataSet.Binaries = binaries;
			questionDb.AttachedDataSet.IsDraft = false;

			managedUser.DataSets.Add(questionDb.AttachedDataSet);
			_dbContext.SaveChanges();

			return new DefaultResponse(null);
		}

		private string? GetFileExtension(string fileName)
		{
			string? originalExtension = Path.GetExtension(fileName)?.ToLowerInvariant();
			if (originalExtension != null && _fileExtensions.Contains(originalExtension))
			{
				return originalExtension;
			}

			return null;
		}

		private DefaultResponse TerminateUploading(DataSet dataSet, string[] errors)
		{
			Directory.Delete(dataSet.GetDataSetDirectory(), true);
			_dbContext.DataSets.Remove(dataSet);
			_dbContext.SaveChanges();
			return new DefaultResponse(errors);
		}

		private static bool IsImage(byte[] buffer)
		{
			if (StartsWithSignature(buffer, PNGSignature))
			{
				return true;
			}

			if (StartsWithSignature(buffer, JPEGSignature))
			{
				return true;
			}

			if (StartsWithSignature(buffer, WebPSignature))
			{
				return true;
			}

			if (StartsWithSignature(buffer, JFIFSignature))
			{
				return true;
			}

			return false;
		}

		private static bool StartsWithSignature(byte[] buffer, byte[] signature)
		{
			if (buffer.Length < signature.Length)
			{
				return false;
			}

			for (int i = 0; i < signature.Length; i++)
			{
				if (buffer[i] != signature[i])
				{
					return false;
				}
			}

			return true;
		}
	}
}
