using MediatR;
using PianoMentor.Contract.Models.PianoMentor.Quizzes;
using PianoMentor.Contract.Quizzes;
using PianoMentor.DAL;
using System.Data.Entity;

namespace PianoMentor.BLL.Quizzes
{
	internal class GetQuizHandler(PianoMentorDbContext dbContext) : IRequestHandler<GetQuizRequest, GetQuizResponse>
	{
		private readonly PianoMentorDbContext _dbContext = dbContext;

		public async Task<GetQuizResponse> Handle(GetQuizRequest request, CancellationToken cancellationToken)
		{
			var questions = _dbContext.QuizQuestions
				.AsNoTracking()
				.Where(q => q.CourseItemId == request.CourseItemId && !q.IsDeleted)
				.Select(q => new QuizQuestionModel
				{
					QuestionId = q.QuestionId,
					QuestionText = q.QuestionText,
					UpdatedAt = q.UpdatedAt,
					QuizQuestionType = q.QuizQuestionType.Name,
					AttachedDataSetId = q.AttachedDataSetId,
					CourseItemId = request.CourseItemId,
					Answers = q.QuizQuestionsAnswers
						.Select(qa => new QuizQuestionAnswerModel
						{
							AnswerId = qa.AnswerId,
							AnswerText = qa.AnswerText,
							IsCorrect = qa.IsCorrect
						})
						.ToList()
				})
				.ToList();

			var lastUserAnswers = await _dbContext.QuizQuestionUserAnswerLogs
				.AsNoTracking()
				.Where(l =>
					l.UserId == request.UserId
					&& questions.SelectMany(q => q.Answers.Select(a => a.AnswerId)).Contains(l.AnswerId)
					&& l.IsCorrect
					&& l.AnsweredAt == _dbContext.QuizQuestionUserAnswerLogs
						.Where(l2 => l2.UserId == request.UserId)
						.Max(l2 => l2.AnsweredAt))
				.ToListAsync();

			foreach (var question in questions)
			{
				foreach (var answer in question.Answers)
				{
					var lastUserAnswer = lastUserAnswers.FirstOrDefault(l => l.AnswerId == answer.AnswerId);
					if (lastUserAnswer == null)
					{
						continue;
					}

					answer.WasChosenByUser = true;
					answer.UserAnswerText = lastUserAnswer.UserAnswerText;
				}
			}

			return new GetQuizResponse(questions, null);
		}
	}
}
