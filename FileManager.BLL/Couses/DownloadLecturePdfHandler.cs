using FileManager.Contract.Courses;
using FileManager.Contract.Files;
using FileManager.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FileManager.BLL.Couses
{
	internal class DownloadLecturePdfHandler(
		FileManagerDbContext dbContext,
		IConfiguration config) : IRequestHandler<DownloadLecturePdfRequest, DownloadFilesResponse>
	{
		private readonly FileManagerDbContext _dbContext = dbContext;
		private readonly string _basePathForTempFiles = config.GetValue<string>("BasePathForTempFiles")
			?? throw new ArgumentNullException("Cannot find base path for temp files from configuration");

		public async Task<DownloadFilesResponse> Handle(DownloadLecturePdfRequest request, CancellationToken cancellationToken)
		{
			if (!Directory.Exists(_basePathForTempFiles))
			{
				Directory.CreateDirectory(_basePathForTempFiles);
			}

			var courseAttach = await _dbContext.CourseItems
				.AsNoTracking()
				.Include(ci => ci.AttachedDataSet)
				.Include(ci => ci.AttachedDataSet.Storage)
				.Include(ci => ci.AttachedDataSet.Binaries)
				.Where(ci => 
					ci.CourseItemId == request.CourseItemId 
					&& !ci.AttachedDataSet.IsDraft
					&& !ci.IsDeleted)
				.Select(ci => new
				{
					ci.AttachedDataSet
				})
				.FirstOrDefaultAsync(cancellationToken);

			if (courseAttach == null)
			{
				return new DownloadFilesResponse(null, null, null, [$"Cannot find a course item attachement"]);
			}

			if (courseAttach.AttachedDataSet == null || courseAttach.AttachedDataSet.Binaries == null || courseAttach.AttachedDataSet.Storage == null)
			{
				return new DownloadFilesResponse(null, null, null, [$"Cannot find a binary in DB for course item'"]);
			}

			var binary = courseAttach.AttachedDataSet.Binaries.FirstOrDefault();
			var file = binary?.GetInternalFile();
			if (file == null || !file.Exists)
			{
				return new DownloadFilesResponse(null, null, null, [$"Cannot find a physical file for course item"]);
			}

			return CreatePdfResponse(file.FullName);
		}

		private static DownloadFilesResponse CreatePdfResponse(string pdfPath)
		{
			// Не используется ключевое слово using у FileStream потому,
			// что FileStreamResult сам вызывает метод Dispose() у потока, после передачи данных из него.
			// Смотри: https://github.com/aspnet/AspNetWebStack/blob/main/src/System.Web.Mvc/FileStreamResult.cs

			var fileStream = new FileStream(pdfPath, FileMode.Open, FileAccess.Read);
			return new DownloadFilesResponse(fileStream, "application/pdf", Path.GetFileName(pdfPath), null);
		}

	}
}
