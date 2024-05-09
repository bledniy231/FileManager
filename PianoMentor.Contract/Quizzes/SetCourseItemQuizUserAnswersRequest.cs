using MediatR;
using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.PianoMentor.Quizzes;

namespace PianoMentor.Contract.Quizzes
{
	public class SetCourseItemQuizUserAnswersRequest : IRequest<DefaultResponse>
	{
		public int CourseId { get; set; }
		public int CourseItemId { get; set; }
		public long UserId { get; set; }
		public bool IsQuizInProgress { get; set; }
		public bool IsQuizPassed { get; set; }
		public bool IsQuizFailed { get; set; }
		public List<QuizQuestionUserAnswerModel> UserAnswers { get; set; }
	}
}
