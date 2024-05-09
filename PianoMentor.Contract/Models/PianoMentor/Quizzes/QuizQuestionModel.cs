namespace PianoMentor.Contract.Models.PianoMentor.Quizzes
{
	public class QuizQuestionModel
	{
		public int QuestionId { get; set; }
		public string QuestionText { get; set; }
		public DateTime UpdatedAt { get; set; }
		public string QuizQuestionType { get; set; }
		public long? AttachedDataSetId { get; set; }
		public int CourseItemId { get; set; }
		public List<QuizQuestionAnswerModel> Answers { get; set; }
	}
}
