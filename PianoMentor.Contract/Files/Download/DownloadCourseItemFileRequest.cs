using MediatR;

namespace PianoMentor.Contract.Files.Download
{
	public class DownloadCourseItemFileRequest(int courseItemId) : IRequest<DownloadFilesResponse>
	{
		public int CourseItemId { get; set; } = courseItemId;
	}
}
