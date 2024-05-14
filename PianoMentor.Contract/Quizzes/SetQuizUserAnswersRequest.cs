using MediatR;
using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.PianoMentor.Quizzes;

namespace PianoMentor.Contract.Quizzes
{
	public class SetQuizUserAnswersRequest : IRequest<SetQuizUserAnswersResponse>
	{
		public int CourseId { get; set; }
		public int CourseItemId { get; set; }
		public long UserId { get; set; }
		/// <summary>
		/// Ставится true, если пользователь отправляет итоговый вариант прохождения теста
		/// Ставится false, если пользователь отправляет промежуточный вариант прохождения теста (чтобы, например, продолжить тестирование позже)
		/// </summary>
		public bool IsQuizCompletedByUser { get; set; }
		public List<QuizQuestionModel> QuestionsWithUserAnswers { get; set; }
	}
}
