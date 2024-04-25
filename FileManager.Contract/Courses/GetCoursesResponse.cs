using FileManager.Contract.Default;
using FileManager.Contract.Models.PianoMentor.Courses;

namespace FileManager.Contract.Courses
{
	public class GetCoursesResponse(List<CourseModel> courses, string[]? errors) : DefaultResponse(errors)
	{
		public List<CourseModel> Courses { get; set; } = courses;
	}
}