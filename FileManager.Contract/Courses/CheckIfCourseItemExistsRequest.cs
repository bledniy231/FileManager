using FileManager.Contract.Default;
using FileManager.Contract.Models.PianoMentor.Courses;
using MediatR;

namespace FileManager.Contract.Courses
{
	public class CheckIfCourseItemExistsRequest : IRequest<DefaultResponse>
	{
		public int CourseItemId { get; set; }
		public int CourseItemTypeId { get; set; }
	}
}
