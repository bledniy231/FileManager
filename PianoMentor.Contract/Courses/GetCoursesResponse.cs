using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.PianoMentor.Courses;

namespace PianoMentor.Contract.Courses
{
	public class GetCoursesResponse(List<CourseModel> courses, string[]? errors) : DefaultResponse(errors)
	{
		public List<CourseModel> Courses { get; set; } = courses;
	}
}