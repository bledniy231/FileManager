using PianoMentor.Contract.Models.PianoMentor.Courses;

namespace PianoMentor.Contract.ApplicationUser
{
	public class GetCoursesUserStatisticsResponse
	{
		public List<CourseUserProgressModel> CoursesUserProgress { get; set; }
	}
}