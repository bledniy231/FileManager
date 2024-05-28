using MediatR;

namespace PianoMentor.Contract.Files
{
	public class DownloadCourseItemFileRequest(int courseItemId) : IRequest<DownloadFilesResponse>
	{
		public int CourseItemId { get; set; } = courseItemId;
	}
}
