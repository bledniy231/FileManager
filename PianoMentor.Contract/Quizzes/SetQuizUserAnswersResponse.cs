using PianoMentor.Contract.Default;

namespace PianoMentor.Contract.Quizzes
{
	public class SetQuizUserAnswersResponse(string progressType, string[] errors) : DefaultResponse(errors)
	{
		public string ProgressType { get; set; } = progressType;
	}
}
