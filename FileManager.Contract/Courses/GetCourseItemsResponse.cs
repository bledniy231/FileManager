using FileManager.Contract.Default;
using FileManager.Contract.Models.PianoMentor.Courses;

namespace FileManager.Contract.Courses
{
	public class GetCourseItemsResponse(List<CourseItemModel> courseItems, string[]? errors) : DefaultResponse(errors)
	{
		public List<CourseItemModel> CourseItems { get; set; } = courseItems;
	}
}