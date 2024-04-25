using FileManager.Contract.Default;
using FileManager.Contract.Models.PianoMentor.Courses;
using MediatR;

namespace FileManager.Contract.Courses
{
	public class SetNewCoursesViaAdminRequest : IRequest<DefaultResponse>
	{
		public long UserId { get; set; }
		public List<CourseModel> CoursesModels { get; set; }
	}
}
