using PianoMentor.Contract.Files;
using MediatR;

namespace PianoMentor.Contract.Courses
{
	public class DownloadCourseItemFileRequest(int courseItemId) : IRequest<DownloadFilesResponse>
	{
		public int CourseItemId { get; set; } = courseItemId;
	}
}
