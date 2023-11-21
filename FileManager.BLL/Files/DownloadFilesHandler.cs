using FileManager.Contract.Files;
using FileManager.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO.Compression;

namespace FileManager.BLL.Files
{
	internal class DownloadFilesHandler(
		FileManagerDbContext dbContext,
		IConfiguration config)
		: IRequestHandler<DownloadFilesRequest, DownloadFilesResponse>
	{
		private readonly FileManagerDbContext _dbContext = dbContext;
		private readonly string _basePathForTempFiles = config.GetValue<string>("BasePathForTempFiles") 
			?? throw new ArgumentNullException("Cannot find base path for temp files from configuration");

		public async Task<DownloadFilesResponse> Handle(DownloadFilesRequest request, CancellationToken cancellationToken)
		{
			if (!Directory.Exists(_basePathForTempFiles))
			{
				Directory.CreateDirectory(_basePathForTempFiles);
			}

			if (request.DataId != null)
			{
				var binary = await _dbContext.Binaries
					.AsNoTracking()
					.Include(b => b.DataSet.Storage)
					.FirstOrDefaultAsync(b => b.DataSetId == request.DataSetId && b.DataId == request.DataId, cancellationToken);

				if (binary == null)
				{
					return new DownloadFilesResponse
					{
						IsSuccess = false,
						Message = $"Cannot find a binary in DB from data set \'{request.DataSetId}\' with data id \'{request.DataId}\'"
					};
				}

				var file = binary.GetInternalFile();
				if (!file.Exists)
				{
					return new DownloadFilesResponse()
					{
						IsSuccess = false,
						Message = $"Cannot find a physical file from data set \'{request.DataSetId}\' with data id \'{request.DataId}\'"
					};
				}

				try
				{
					return await CreateResponse(async () =>
					{
						string tempZipPath = Path.Combine(_basePathForTempFiles, $"{Guid.NewGuid()}_{request.DataSetId}_{request.DataId}.zip");
						var zipMode = ZipArchiveMode.Create;
						if (File.Exists(tempZipPath))
						{
							zipMode = ZipArchiveMode.Read;
						}

						using (var archive = ZipFile.Open(tempZipPath, zipMode))
						{
							await Task.Run(() => archive.CreateEntryFromFile(file.FullName, file.Name));
						}

						return tempZipPath;
					});
				}
				catch (Exception ex)
				{
					return new DownloadFilesResponse()
					{
						IsSuccess = false,
						Message = ex.Message
					};
				}
			}
			else
			{
				var dataSet = await _dbContext.DataSets
					.AsNoTracking()
					.Include(d => d.Binaries)
					.Include(d => d.Storage)
					.FirstOrDefaultAsync(d => d.Id == request.DataSetId);

				if (dataSet == null)
				{
					return new DownloadFilesResponse()
					{
						IsSuccess = false,
						Message = $"Cannot find a data set in DB from data set '{request.DataSetId}'"
					};
				}

				try
				{
					return await CreateResponse(async () =>
					{
						string tempZipPath = Path.Combine(_basePathForTempFiles, $"{Guid.NewGuid()}_{request.DataSetId}.zip");
						string dataSetDirName = dataSet.GetDataSetDirectory();
						await Task.Run(() => ZipFile.CreateFromDirectory(dataSetDirName, tempZipPath));
						return tempZipPath;
					});
				}
				catch (Exception ex)
				{
					return new DownloadFilesResponse()
					{
						IsSuccess = false,
						Message = ex.Message
					};
				}
			}
		}

		private async Task<DownloadFilesResponse> CreateResponse(Func<Task<string>> zipFunc)
		{
			// Не используется ключевое слово using у FileStream потому,
			// что FileStreamResult сам вызывает метод Dispose() у потока, после передачи данных из него.
			// Смотри: https://github.com/aspnet/AspNetWebStack/blob/main/src/System.Web.Mvc/FileStreamResult.cs

			string tempZipPath = await zipFunc();
			var fileStream = new FileStream(tempZipPath, FileMode.Open, FileAccess.Read);

			return new DownloadFilesResponse()
			{
				IsSuccess = true,
				FileStream = fileStream,
				FileDownloadName = Path.GetFileName(tempZipPath),
				ContentType = "application/zip"
			};
		}
	}
}
