namespace PianoMentor.Contract.Models.PianoMentor.Quizzes
{
	public class QuizQuestionUserAnswerModel
	{
		public Guid AnswerLogId { get; set; }
		public int QuestionId { get; set; }
		public string? QuestionText { get; set; }
		public int AnswerId { get; set; }
		public long UserId { get; set; }
		public string? UserAnswerText { get; set; }
	}
}
