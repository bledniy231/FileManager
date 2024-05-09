using MediatR;
using PianoMentor.Contract.Default;
using PianoMentor.Contract.Quizzes;
using PianoMentor.DAL;

namespace PianoMentor.BLL.Quizzes
{
	internal class SetCourseItemQuizUserAnswersHandler(PianoMentorDbContext dbContext) : IRequestHandler<SetCourseItemQuizUserAnswersRequest, DefaultResponse>
	{
		private readonly PianoMentorDbContext _dbContext = dbContext;

		public Task<DefaultResponse> Handle(SetCourseItemQuizUserAnswersRequest request, CancellationToken cancellationToken)
		{
			var didUserAlreadyCompletedQuiz = _dbContext.QuizQuestionUserAnswerLogs
				.Where(al =>
					al.UserId == request.UserId 
					&& al.IsCorrect)
		}
	}
}
