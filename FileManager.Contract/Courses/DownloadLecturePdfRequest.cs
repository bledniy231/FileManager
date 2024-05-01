using FileManager.Contract.Files;
using MediatR;

namespace FileManager.Contract.Courses
{
	public class DownloadLecturePdfRequest(int courseItemId) : IRequest<DownloadFilesResponse>
	{
		public int CourseItemId { get; set; } = courseItemId;
	}
}
