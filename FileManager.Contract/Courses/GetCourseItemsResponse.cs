using FileManager.Contract.Models.PianoMentor.Courses;

namespace FileManager.Contract.Courses
{
	public class GetCourseItemsResponse
	{
		public List<CourseItemModel> CourseItems { get; set; }
	}
}