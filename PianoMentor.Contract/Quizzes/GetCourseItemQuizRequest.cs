using MediatR;

namespace PianoMentor.Contract.Quizzes
{
	public class GetCourseItemQuizRequest : IRequest<GetCourseItemQuizResponse>
	{
		public int CourseId { get; set; }
		public int CourseItemId { get; set; }
		public long UserId { get; set; }
	}
}
