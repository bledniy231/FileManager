namespace PianoMentor.Contract.Models.PianoMentor.Quizzes
{
	public class QuizQuestionModel
	{
		public int QuestionId { get; set; }
		public string QuestionText { get; set; }
		/// <summary>
		/// Nullable потому, что используется модель и для сохранения пользовательских ответов, и для отдачи вопросов клиенту
		/// </summary>
		public DateTime? UpdatedAt { get; set; }
		public string QuizQuestionType { get; set; }
		public long? AttachedDataSetId { get; set; }
		public int CourseItemId { get; set; }
		public List<QuizQuestionAnswerModel> Answers { get; set; }
	}
}
