using PianoMentor.BLL.MultipartRequestHelper;
using PianoMentor.BLL.UploadPercentageChecker;
using PianoMentor.Contract.Default;
using PianoMentor.Contract.Files;
using PianoMentor.DAL;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace PianoMentor.BLL.Files
{
	internal class UploadFilesHandler(
		PianoMentorDbContext dbContext,
		FormOptions formOptions,
		IMultipartRequestHelper multipartRequestHelper,
		IPercentageChecker percentageChecker) 
		: IRequestHandler<UploadFilesRequest, DefaultResponse>
	{
		private readonly IMultipartRequestHelper _multipartRequestHelper = multipartRequestHelper;
		private readonly IPercentageChecker _percentageChecker = percentageChecker;
		private readonly PianoMentorDbContext _dbContext = dbContext;
		private readonly FormOptions _formOptions = formOptions;
		private readonly List<string> _failedLoadFilesWithErrors = [];

		public async Task<DefaultResponse> Handle(UploadFilesRequest request, CancellationToken cancellationToken)
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

			var createdDateTime = DateTime.UtcNow;

			// Загрузка прикреплённого файла к CourseItem и загрузка пользовательского файла выполняются одним обработчиком (этим классом)
			// Далее видно ветвление, позволяющее разграничить функциональность для того или иного случая
			if (request.CourseItemId.HasValue)
			{
				var courseItem = _dbContext.CourseItems.FirstOrDefault(ci => ci.CourseItemId == request.CourseItemId && !ci.IsDeleted);
				if (courseItem == null)
				{
					return new DefaultResponse(["Incorrect course item Id or course item was deleted"]);
				}

				courseItem.AttachedDataSet = new DAL.Domain.DataSet.DataSet
				{
					CreatedAt = createdDateTime,
					IsDraft = true,
					Owner = managedUser,
					Storage = storage
				};
				_dbContext.SaveChanges();

				return await UploadFilesInternal(request, courseItem.AttachedDataSet, managedUser, true, cancellationToken);
			}
			else
			{
				var dataSet = new DAL.Domain.DataSet.DataSet
				{
					CreatedAt = createdDateTime,
					IsDraft = true,
					Owner = managedUser,
					Storage = storage
				};
				_dbContext.DataSets.Add(dataSet);
				_dbContext.SaveChanges();

				return await UploadFilesInternal(request, dataSet, managedUser, false, cancellationToken);
			}
		}

		private async Task<DefaultResponse> UploadFilesInternal(
			UploadFilesRequest request,
			DAL.Domain.DataSet.DataSet dataSet,
			DAL.Domain.Identity.PianoMentorUser managedUser,
			bool isAttachedToCourseItem,
			CancellationToken cancellationToken)
		{
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
				if (isAttachedToCourseItem && fileIndex > 0)
				{
					_failedLoadFilesWithErrors.Add("Only one file can be attached to the course item");
					break;
				}

				fileIndex++;

				if (ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition))
				{
					if (!_multipartRequestHelper.HasFileContentDisposition(contentDisposition))
					{
						_failedLoadFilesWithErrors.Add($"File index: {fileIndex}, " +
							$"Error: Wrong file content disposition, it should be like that: " +
							$"Content-Disposition: form-data; name=\"myfile1\"; filename=\"Misc 002.jpg\"");
					}

					string? trustedFileNameForDisplay = WebUtility.HtmlEncode(contentDisposition.FileName.Value);
					if (trustedFileNameForDisplay == null)
					{
						return new DefaultResponse(["Cannot encode file name from Content Disposition"]);
					}

					string? fileExtension = GetFileExtension(trustedFileNameForDisplay, isAttachedToCourseItem);
					if (fileExtension == null)
					{
						_failedLoadFilesWithErrors.Add($"File index: {fileIndex}, Error: Wrong file extension or totaly no file extension");
						section = await reader.ReadNextSectionAsync(cancellationToken);
						continue;
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

						byte[] buffer = new byte[2 * 1024 * 1024];
						int readBytes = 0;

						using var targetStream = File.Create(file.FullName);
						bool isFirstReadingOfBytes = true;
						while ((readBytes = section.Body.ReadAtLeast(buffer, 2 * 1024 * 1024, false)) > 0)
						//while ((readBytes = await section.Body.ReadAtLeastAsync(buffer, 10 * 1024 * 1024, false, cancellationToken).ConfigureAwait(false)) > 0)
						{
							if (isAttachedToCourseItem && isFirstReadingOfBytes)
							{
								isFirstReadingOfBytes = false;
								// Проверка на то, что это реально PDF-файл
								if (buffer[0] != 0x25 || // %
									buffer[1] != 0x50 || // P
									buffer[2] != 0x44 || // D
									buffer[3] != 0x46)   // F
								{
									_failedLoadFilesWithErrors.Add($"File name: {file.Name}, Error: File is not a PDF");
									break;
								}
							}
							//await targetStream.WriteAsync(buffer.AsMemory(0, readBytes), cancellationToken).ConfigureAwait(false);
							targetStream.Write(buffer, 0, readBytes);
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

			if (fileIndex == _failedLoadFilesWithErrors.Count || (isAttachedToCourseItem && _failedLoadFilesWithErrors.Count > 0))
			{
				Directory.Delete(dataSet.GetDataSetDirectory(), true);
				_dbContext.DataSets.Remove(dataSet);
				_dbContext.SaveChanges();
				_failedLoadFilesWithErrors.Add("All files failed to load");
				return new DefaultResponse([.. _failedLoadFilesWithErrors]);
			}

			dataSet.Binaries = binaries;
			dataSet.IsDraft = false;

			managedUser.DataSets.Add(dataSet);
			_dbContext.SaveChanges();

			if (_failedLoadFilesWithErrors.Count != 0)
			{
				_failedLoadFilesWithErrors.Add("Some files failed to load");
				return new DefaultResponse([.. _failedLoadFilesWithErrors]);
			}

			_percentageChecker.RemoveElement(request.UserId);

			return new DefaultResponse(null);
		}

		private static string? GetFileExtension(string fileName, bool isAttachedToCourseItem)
		{
			if (isAttachedToCourseItem)
			{
				string? originalExtension = Path.GetExtension(fileName)?.ToLowerInvariant();
				if (originalExtension != null && originalExtension.Equals(".pdf"))
				{
					return originalExtension;
				}

				return null;
			}

			return Path.GetExtension(fileName)?.ToLowerInvariant();
		}
	}
}
