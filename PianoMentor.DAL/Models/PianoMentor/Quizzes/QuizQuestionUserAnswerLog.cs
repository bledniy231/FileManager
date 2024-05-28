using PianoMentor.DAL.Models.Identity;

namespace PianoMentor.DAL.Models.PianoMentor.Quizzes
{
	public class QuizQuestionUserAnswerLog
	{
		public Guid AnswerLogId { get; set; } 
		public int QuestionId { get; set; }
		public int AnswerId { get; set; }
		public long UserId { get; set; }
		public bool IsCorrect { get; set; }
		public string? UserAnswerText { get; set; }
		public DateTime AnsweredAt { get; set; }
		public virtual QuizQuestion Question { get; set; }
		public virtual QuizQuestionAnswer Answer { get; set; }
		public virtual PianoMentorUser User { get; set; }
	}
}
