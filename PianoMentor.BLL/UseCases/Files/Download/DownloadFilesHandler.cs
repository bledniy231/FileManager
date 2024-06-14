using System.IO.Compression;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PianoMentor.Contract.Files.Download;
using PianoMentor.DAL;

namespace PianoMentor.BLL.UseCases.Files.Download
{
	internal class DownloadFilesHandler(
		PianoMentorDbContext dbContext,
		IConfiguration config)
		: IRequestHandler<DownloadFilesRequest, DownloadFilesResponse>
	{
		private readonly string _baseDirNameForTempFiles = config.GetValue<string>("BaseDirNameForTempFiles") 
		                                                   ?? throw new ArgumentNullException("Cannot find base path for temp files from configuration");

		public async Task<DownloadFilesResponse> Handle(DownloadFilesRequest request, CancellationToken cancellationToken)
		{
			if (!request.IsZipArchiveResponse && request.DataId == null)
			{
				return new DownloadFilesResponse(null, null, null, ["Cannot download whole data set without a zip archive, change flag IsZipArchiveResponse=true"]);
			}

			if (request.DataId != null)
			{
				var binary = await dbContext.Binaries
					.AsNoTracking()
					.Include(b => b.DataSet.Storage)
					.FirstOrDefaultAsync(b => b.DataSetId == request.DataSetId && b.DataId == request.DataId, cancellationToken);
				if (binary == null)
				{
					return new DownloadFilesResponse(null, null, null, [$"Cannot find a binary in DB from data set \'{request.DataSetId}\' with data id \'{request.DataId}\'"]);
				}

				var file = binary.GetInternalFile();
				if (!file.Exists)
				{
					return new DownloadFilesResponse(null, null, null, [$"Cannot find a physical file from data set \'{request.DataSetId}\' with data id \'{request.DataId}\'"]);
				}

				string basePathForTempFiles = Path.Combine(Directory.GetParent(binary.DataSet.Storage.BasePath).FullName, _baseDirNameForTempFiles);
				if (!Directory.Exists(basePathForTempFiles))
				{
					Directory.CreateDirectory(basePathForTempFiles);
				}

				try
				{
					if (request.IsZipArchiveResponse)
					{
						return CreateResponse(() =>
						{
							string tempZipPath = Path.Combine(basePathForTempFiles, $"{Guid.NewGuid()}_{request.DataSetId}_{request.DataId}.zip");
							var zipMode = ZipArchiveMode.Create;
							if (File.Exists(tempZipPath))
							{
								zipMode = ZipArchiveMode.Read;
							}

							using var archive = ZipFile.Open(tempZipPath, zipMode);
							archive.CreateEntryFromFile(file.FullName, file.Name);

							return tempZipPath;
						});
					}
					
					return CreateResponse(() => file.FullName);
				}
				catch (Exception ex)
				{
					return new DownloadFilesResponse(null, null, null, [ex.Message]);
				}
			}
			else
			{
				var dataSet = await dbContext.DataSets
					.AsNoTracking()
					.Include(d => d.Binaries)
					.Include(d => d.Storage)
					.FirstOrDefaultAsync(d => d.Id == request.DataSetId, cancellationToken);

				if (dataSet == null)
				{
					return new DownloadFilesResponse(null, null, null, [$"Cannot find a data set in DB from data set '{request.DataSetId}'"]);
				}

				string basePathForTempFiles = Path.Combine(Directory.GetParent(dataSet.Storage.BasePath).FullName, _baseDirNameForTempFiles);
				if (!Directory.Exists(basePathForTempFiles))
				{
					Directory.CreateDirectory(basePathForTempFiles);
				}

				try
				{
					return CreateResponse(() =>
					{
						string tempZipPath = Path.Combine(basePathForTempFiles, $"{Guid.NewGuid()}_{request.DataSetId}.zip");
						string dataSetDirName = dataSet.GetDataSetDirectory();
						ZipFile.CreateFromDirectory(dataSetDirName, tempZipPath);
						return tempZipPath;
					});
				}
				catch (Exception ex)
				{
					return new DownloadFilesResponse(null, null, null, [ex.Message]);
				}
			}
		}

		private static DownloadFilesResponse CreateResponse(Func<string> func)
		{
			// Не используется ключевое слово using у FileStream потому,
			// что FileStreamResult сам вызывает метод Dispose() у потока, после передачи данных из него.
			// Смотри: https://github.com/aspnet/AspNetWebStack/blob/main/src/System.Web.Mvc/FileStreamResult.cs

			string tempPath = func();
			var fileStream = new FileStream(tempPath, FileMode.Open, FileAccess.Read);
			
			var provider = new FileExtensionContentTypeProvider();
			if(!provider.TryGetContentType(tempPath, out var contentType))
			{
				contentType = "application/octet-stream";
			}

			return new DownloadFilesResponse(fileStream, contentType, Path.GetFileName(tempPath), null);
		}
	}
}
