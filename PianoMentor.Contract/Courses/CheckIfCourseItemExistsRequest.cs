using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.PianoMentor.Courses;
using MediatR;

namespace PianoMentor.Contract.Courses
{
	public class CheckIfCourseItemExistsRequest : IRequest<DefaultResponse>
	{
		public int CourseItemId { get; set; }
		public int CourseItemTypeId { get; set; }
	}
}
