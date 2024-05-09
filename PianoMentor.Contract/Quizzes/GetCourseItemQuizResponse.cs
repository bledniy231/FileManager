using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.PianoMentor.Quizzes;

namespace PianoMentor.Contract.Quizzes
{
	public class GetCourseItemQuizResponse(List<QuizQuestionModel> models, string[]? errors) : DefaultResponse(errors)
	{
		public List<QuizQuestionModel> QuestionModels { get; set; } = models;
	}
}