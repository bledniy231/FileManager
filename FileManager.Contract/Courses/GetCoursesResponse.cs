using FileManager.Contract.Models.PianoMentor.Courses;

namespace FileManager.Contract.Courses
{
	public class GetCoursesResponse
	{
		public List<CourseModel> Courses { get; set; }
	}
}