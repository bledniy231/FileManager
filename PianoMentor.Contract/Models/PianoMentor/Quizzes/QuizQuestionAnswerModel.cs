namespace PianoMentor.Contract.Models.PianoMentor.Quizzes
{
	public class QuizQuestionAnswerModel
	{
		public int AnswerId { get; set; }
		public string AnswerText { get; set; }
		public bool IsCorrect { get; set; }
	}
}
