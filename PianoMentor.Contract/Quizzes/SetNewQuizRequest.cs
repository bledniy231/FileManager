using MediatR;
using PianoMentor.Contract.Default;

namespace PianoMentor.Contract.Quizzes
{
	public class SetNewQuizRequest : IRequest<DefaultResponse>
	{
		public List<QuestionModelForInsert> Questions { get; set; }
	}

	public class QuestionModelForInsert
	{
		public string QuestionText { get; set; }
		public int CourseItemId { get; set; }
		public int QuizQuestionTypeId { get; set; }
		public List<AnswerModelForInsert> Answers { get; set; }
	}

	public class AnswerModelForInsert
	{
		public string AnswerText { get; set; }
		public bool IsCorrect { get; set; }
	}
}
