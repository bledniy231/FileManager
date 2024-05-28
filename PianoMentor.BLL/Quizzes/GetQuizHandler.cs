using MediatR;
using Microsoft.EntityFrameworkCore;
using PianoMentor.Contract.Models.PianoMentor.Quizzes;
using PianoMentor.Contract.Quizzes;
using PianoMentor.DAL;

namespace PianoMentor.BLL.Quizzes
{
	internal class GetQuizHandler(PianoMentorDbContext dbContext) : IRequestHandler<GetQuizRequest, GetQuizResponse>
	{
		public async Task<GetQuizResponse> Handle(GetQuizRequest request, CancellationToken cancellationToken)
		{
			var questions = dbContext.QuizQuestions
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

			var allAnswersIds = questions.SelectMany(q => q.Answers.Select(a => a.AnswerId)).ToList();
			var lastUserAnswers = await dbContext.QuizQuestionUserAnswerLogs
				.AsNoTracking()
				.Where(al =>
					al.UserId == request.UserId
					&& allAnswersIds.Contains(al.AnswerId)
					&& al.AnsweredAt == dbContext.QuizQuestionUserAnswerLogs
						.Where(al2 => al2.UserId == request.UserId)
						.Max(al2 => al2.AnsweredAt))
				.ToListAsync(cancellationToken);

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
