namespace PianoMentor.DAL.Models.PianoMentor.Quizzes
{
	public class QuizQuestionAnswer
	{
		public int AnswerId { get; set; }
		public string AnswerText { get; set; }
		public bool IsCorrect { get; set; }
		public bool IsDeleted { get; set; }
		public int QuizQuestionId { get; set; }
		public virtual QuizQuestion QuizQuestions { get; set; }
	}
}