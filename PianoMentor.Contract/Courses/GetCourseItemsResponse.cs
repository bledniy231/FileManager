using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.PianoMentor.Courses;

namespace PianoMentor.Contract.Courses
{
	public class GetCourseItemsResponse(List<CourseItemModel> courseItems, string[]? errors) : DefaultResponse(errors)
	{
		public List<CourseItemModel> CourseItems { get; set; } = courseItems;
	}
}