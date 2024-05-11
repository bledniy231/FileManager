using MediatR;
using PianoMentor.Contract.Default;
using PianoMentor.Contract.Quizzes;
using PianoMentor.DAL;
using PianoMentor.DAL.Models.PianoMentor.Quizzes;

namespace PianoMentor.BLL.Quizzes
{
	internal class SetNewQuizHandler(PianoMentorDbContext dbContext) : IRequestHandler<SetNewQuizRequest, DefaultResponse>
	{
		private readonly PianoMentorDbContext _dbContext = dbContext;

		public Task<DefaultResponse> Handle(SetNewQuizRequest request, CancellationToken cancellationToken)
		{
			var updatedAt = DateTime.UtcNow;
			var questionsDb = request.Questions
				.Select(q => new QuizQuestion
				{
					QuestionText = q.QuestionText,
					UpdatedAt = updatedAt,
					IsDeleted = false,
					CourseItemId = q.CourseItemId,
					QuizQuestionTypeId = q.QuizQuestionTypeId,
					QuizQuestionsAnswers = q.Answers.Select(a => new QuizQuestionAnswer
					{
						AnswerText = a.AnswerText,
						IsCorrect = a.IsCorrect,
						IsDeleted = false
					}).ToList()
				}).ToList();

			try
			{
				_dbContext.QuizQuestions.AddRange(questionsDb);
				_dbContext.SaveChanges();
			}
			catch (Exception ex)
			{
				return Task.FromResult(new DefaultResponse([ex.Message]));
			}

			return Task.FromResult(new DefaultResponse(null));
		}
	}
}
