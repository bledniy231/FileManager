using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.PianoMentor.Courses;
using MediatR;

namespace PianoMentor.Contract.Courses
{
	public class SetNewCoursesViaAdminRequest : IRequest<DefaultResponse>
	{
		public long UserId { get; set; }
		public List<CourseModel> CoursesModels { get; set; }
	}
}
