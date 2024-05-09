using PianoMentor.DAL.Domain.DataSet;
using PianoMentor.DAL.Domain.PianoMentor.Courses;

namespace PianoMentor.DAL.Models.PianoMentor.Quizzes
{
	public class QuizQuestion
	{
		public int QuestionId { get; set; }
		public string QuestionText { get; set; }
		public DateTime UpdatedAt { get; set; }
		public bool IsDeleted { get; set; }
		public long? AttachedDataSetId { get; set; }
		public int CourseItemId { get; set; }
		public int QuizQuestionTypeId { get; set; }
		public virtual DataSet AttachedDataSet { get; set; }
		public virtual CourseItem CourseItem { get; set; }
		public virtual List<QuizQuestionAnswer> QuizQuestionsAnswers { get; set; }
		public virtual QuizQuestionType QuizQuestionType { get; set; }
	}
}
