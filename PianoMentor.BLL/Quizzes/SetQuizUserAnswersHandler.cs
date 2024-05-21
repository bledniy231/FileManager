using MediatR;
using Microsoft.EntityFrameworkCore;
using PianoMentor.Contract.Models.PianoMentor.Courses;
using PianoMentor.Contract.Models.PianoMentor.Quizzes;
using PianoMentor.Contract.Quizzes;
using PianoMentor.DAL;
using PianoMentor.DAL.Domain.PianoMentor.Courses;
using PianoMentor.DAL.Models.PianoMentor.Quizzes;

namespace PianoMentor.BLL.Quizzes
{
	internal class SetQuizUserAnswersHandler(PianoMentorDbContext dbContext) : IRequestHandler<SetQuizUserAnswersRequest, SetQuizUserAnswersResponse>
	{
		private readonly PianoMentorDbContext _dbContext = dbContext;

		public async Task<SetQuizUserAnswersResponse> Handle(SetQuizUserAnswersRequest request, CancellationToken cancellationToken)
		{
			bool didUserAlreadyCompletedQuiz = await _dbContext.QuizQuestionUserAnswerLogs
				.CountAsync(al =>
					al.UserId == request.UserId
					&& al.IsCorrect
					&& !al.Question.IsDeleted
					&& al.Question.CourseItemId == request.CourseItemId
					&& al.AnsweredAt == _dbContext.QuizQuestionUserAnswerLogs
						.Where(al2 => al2.UserId == request.UserId)
						.Max(al2 => al2.AnsweredAt), cancellationToken)
				==
				_dbContext.QuizQuestions.Count(q => q.CourseItemId == request.CourseItemId && !q.IsDeleted);

			if (didUserAlreadyCompletedQuiz)
			{
				return new SetQuizUserAnswersResponse(CourseItemProgressTypesEnumaration.Completed.ToString(), ["User already completed the quiz"]);
			}

			var questionsFromDb = _dbContext.QuizQuestions
				.Include(q => q.QuizQuestionsAnswers)
				.Where(q =>
					q.CourseItemId == request.CourseItemId
					&& !q.IsDeleted)
				.ToList();

			//var questionsFromDbAnon = _dbContext.QuizQuestions
			//	.Where(q =>
			//		q.CourseItemId == request.CourseItemId
			//		&& !q.IsDeleted)
			//	.Select(q => new
			//	{
			//		Question = q,
			//		CorrectAnswers = q.QuizQuestionsAnswers.Where(a => a.IsCorrect).ToList()
			//	})
			//	.ToList();

			//foreach (var item in questionsFromDbAnon)
			//{
			//	item.Question.QuizQuestionsAnswers = item.CorrectAnswers;
			//}

			//var questionsWithCorrectAnswers = questionsFromDbAnon.Select(item => item.Question).ToList();
			//if (questionsWithCorrectAnswers == null || questionsWithCorrectAnswers.Count == 0)
			//{
			//	return new DefaultResponse(["No questions with correct answers found in DB"]);
			//}

			if (!request.IsQuizCompletedByUser)
			{
				try
				{
					await SaveUserAnswers(request, questionsFromDb, (int)CourseItemProgressTypesEnumaration.InProgress, cancellationToken);
					return new SetQuizUserAnswersResponse(CourseItemProgressTypesEnumaration.InProgress.ToString(), null);
				}
				catch (Exception ex)
				{
					return new SetQuizUserAnswersResponse(CourseItemProgressTypesEnumaration.NotStarted.ToString(), [ex.Message]);
				}
			}

			int countOfUserCorrectQuestions = 0;
			List<string> errors = [];

			foreach (var userQuestion in request.QuestionsWithUserAnswers)
			{
				var dbQuestion = questionsFromDb.FirstOrDefault(q => q.QuestionId == userQuestion.QuestionId);
				if (dbQuestion == null)
				{
					errors.Add($"Question with id {userQuestion.QuestionId} not found");
					continue;
				}
				var dbCorrectAnswers = dbQuestion.QuizQuestionsAnswers.Where(a => a.IsCorrect && !a.IsDeleted).ToList();
				if (dbCorrectAnswers.Count != userQuestion.Answers.Count(a => a.WasChosenByUser.HasValue && (bool)a.WasChosenByUser))
				{
					// Если количество ответов пользователя отличается от количества правильных ответов,
					// то получается, что ответ пользователя уже неправильный, переходим к следующему вопросу
					continue;
				}

				try
				{
					if (dbQuestion.QuizQuestionTypeId == (int)QuizQuestionTypeEnumeration.FreeText
						&& dbCorrectAnswers.Single().AnswerText.ToLowerInvariant()
							.Equals(userQuestion.Answers.SingleOrDefault()?.UserAnswerText?.ToLowerInvariant() ?? ""))
					{
						countOfUserCorrectQuestions++;
						continue;
					}
				}
				catch (Exception ex)
				{
					return new SetQuizUserAnswersResponse(CourseItemProgressTypesEnumaration.NotStarted.ToString(), [ex.Message]);
				}

				int countOfUserCorrectQuestionAnswers = 0;
				foreach (var dbCorrectAnswer in dbCorrectAnswers)
				{
					if (userQuestion.Answers.Any(a => a.AnswerId == dbCorrectAnswer.AnswerId && a.WasChosenByUser.HasValue && (bool)a.WasChosenByUser))
					{
						countOfUserCorrectQuestionAnswers++;
					}
					else
					{
						// Если не встретили среди правильных ответов тот, что дал пользователь,
						// то ответ пользователя неправильный, переходим к следующему вопросу
						break;
					}
				}

				if (countOfUserCorrectQuestionAnswers == dbCorrectAnswers.Count)
				{
					countOfUserCorrectQuestions++;
				}
			}

			if (errors.Count > 0)
			{
				errors.Add("No answers was saved!");
				return new SetQuizUserAnswersResponse(CourseItemProgressTypesEnumaration.NotStarted.ToString(), errors.ToArray());
			}

			try
			{
				if (countOfUserCorrectQuestions == questionsFromDb.Count)
				{
					await SaveUserAnswers(request, questionsFromDb, (int)CourseItemProgressTypesEnumaration.Completed, cancellationToken);
					return new SetQuizUserAnswersResponse(CourseItemProgressTypesEnumaration.Completed.ToString(), null);
				}
				else
				{
					await SaveUserAnswers(request, questionsFromDb, (int)CourseItemProgressTypesEnumaration.Failed, cancellationToken);
					return new SetQuizUserAnswersResponse(CourseItemProgressTypesEnumaration.Failed.ToString(), null);
				}
			}
			catch (Exception ex)
			{
				return new SetQuizUserAnswersResponse(CourseItemProgressTypesEnumaration.NotStarted.ToString(), [ex.Message]);
			}
		}

		private async Task SaveUserAnswers(
			SetQuizUserAnswersRequest request, 
			List<QuizQuestion> questionsWithCorrectAnswers, 
			int courseItemProgressTypeId,
			CancellationToken cancellationToken)
		{
			var answeredAt = DateTime.UtcNow;
			var userAnswersLogs = new List<QuizQuestionUserAnswerLog>();
			foreach (var userQuestion in request.QuestionsWithUserAnswers)
			{
				var userAnswersByQuestion = userQuestion.Answers
					.Where(a => a.WasChosenByUser.HasValue && (bool)a.WasChosenByUser)
					.Select(a => new QuizQuestionUserAnswerLog
					{
						QuestionId = userQuestion.QuestionId,
						AnswerId = a.AnswerId,
						UserId = request.UserId,
						IsCorrect = questionsWithCorrectAnswers
							.First(qc => qc.QuestionId == userQuestion.QuestionId)
							.QuizQuestionsAnswers
							.First(ans => ans.AnswerId == a.AnswerId)
							.IsCorrect,
						UserAnswerText = a.UserAnswerText,
						AnsweredAt = answeredAt
					}).ToList();
				userAnswersLogs.AddRange(userAnswersByQuestion);
			}

			_dbContext.QuizQuestionUserAnswerLogs.AddRange(userAnswersLogs);
			var courseItemUserProgress = new CourseItemUserProgress
			{
				UserId = request.UserId,
				CourseItemId = request.CourseItemId,
				CourseItemProgressTypeId = courseItemProgressTypeId,
				CreatedAt = answeredAt
			};
			_dbContext.CourseItemUserProgresses.Add(courseItemUserProgress);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
