using MediatR;
using PianoMentor.Contract.Default;

namespace PianoMentor.Contract.Courses
{
	public class SetCourseItemProgressRequest : IRequest<DefaultResponse>
	{
		public long UserId { get; set; }
		public int CourseId { get; set; }
		public int CourseItemId { get; set; }
		public string CourseItemProgressType { get; set; }
	}
}
