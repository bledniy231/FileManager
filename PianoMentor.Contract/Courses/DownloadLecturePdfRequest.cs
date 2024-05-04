using PianoMentor.Contract.Files;
using MediatR;

namespace PianoMentor.Contract.Courses
{
	public class DownloadLecturePdfRequest(int courseItemId) : IRequest<DownloadFilesResponse>
	{
		public int CourseItemId { get; set; } = courseItemId;
	}
}
