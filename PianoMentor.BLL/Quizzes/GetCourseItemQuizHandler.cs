using MediatR;
using PianoMentor.Contract.Models.PianoMentor.Quizzes;
using PianoMentor.Contract.Quizzes;
using PianoMentor.DAL;
using System.Data.Entity;

namespace PianoMentor.BLL.Quizzes
{
	internal class GetCourseItemQuizHandler(PianoMentorDbContext dbContext) : IRequestHandler<GetCourseItemQuizRequest, GetCourseItemQuizResponse>
	{
		private readonly PianoMentorDbContext _dbContext = dbContext;

		public Task<GetCourseItemQuizResponse> Handle(GetCourseItemQuizRequest request, CancellationToken cancellationToken)
		{
			var questions = _dbContext.QuizQuestions
				.AsNoTracking()
				.Include(q => q.AttachedDataSet)
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

			return Task.FromResult(new GetCourseItemQuizResponse(questions, null));
		}
	}
}
