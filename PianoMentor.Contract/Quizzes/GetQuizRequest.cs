using MediatR;

namespace PianoMentor.Contract.Quizzes
{
	public class GetQuizRequest : IRequest<GetQuizResponse>
	{
		public int CourseId { get; set; }
		public int CourseItemId { get; set; }
		public long UserId { get; set; }
	}
}
