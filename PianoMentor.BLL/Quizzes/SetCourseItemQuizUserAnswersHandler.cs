using MediatR;
using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.PianoMentor.Courses;
using PianoMentor.Contract.Quizzes;
using PianoMentor.DAL;
using PianoMentor.DAL.Domain.PianoMentor.Courses;
using PianoMentor.DAL.Models.PianoMentor.Quizzes;

namespace PianoMentor.BLL.Quizzes
{
	internal class SetCourseItemQuizUserAnswersHandler(PianoMentorDbContext dbContext) : IRequestHandler<SetCourseItemQuizUserAnswersRequest, DefaultResponse>
	{
		private readonly PianoMentorDbContext _dbContext = dbContext;

		public async Task<DefaultResponse> Handle(SetCourseItemQuizUserAnswersRequest request, CancellationToken cancellationToken)
		{
			bool didUserAlreadyCompletedQuiz = _dbContext.QuizQuestionUserAnswerLogs
				.Count(al =>
					al.UserId == request.UserId
					&& al.IsCorrect
					&& !al.Question.IsDeleted
					&& al.Question.CourseItemId == request.CourseItemId)
				==
				_dbContext.QuizQuestions.Count(q => q.CourseItemId == request.CourseItemId && !q.IsDeleted);

			if (didUserAlreadyCompletedQuiz)
			{
				return new DefaultResponse(["User already completed the quiz"]);
			}

			var questionsFromDbAnon = _dbContext.QuizQuestions
				.Where(q =>
					q.CourseItemId == request.CourseItemId
					&& !q.IsDeleted)
				.Select(q => new
				{
					Question = q,
					CorrectAnswers = q.QuizQuestionsAnswers.Where(a => a.IsCorrect).ToList()
				})
				.ToList();

			foreach (var item in questionsFromDbAnon)
			{
				item.Question.QuizQuestionsAnswers = item.CorrectAnswers;
			}

			var questionsWithCorrectAnswers = questionsFromDbAnon.Select(item => item.Question).ToList();
			if (questionsWithCorrectAnswers == null || questionsWithCorrectAnswers.Count == 0)
			{
				return new DefaultResponse(["No questions with correct answers found in DB"]);
			}

			if (!request.IsQuizCompletedByUser)
			{
				try
				{
					await SaveUserAnswers(request, questionsWithCorrectAnswers, (int)CourseItemProgressTypesEnumaration.InProgress, cancellationToken);
					return new DefaultResponse(null);
				}
				catch (Exception ex)
				{
					return new DefaultResponse([ex.Message]);
				}
			}

			int countOfUserCorrectQuestions = 0;
			List<string> errors = [];

			foreach (var userQuestionWithAnswers in request.QuestionsWithUserAnswers)
			{
				var neededQuestion = questionsWithCorrectAnswers.FirstOrDefault(q => q.QuestionId == userQuestionWithAnswers.QuestionId);
				if (neededQuestion == null)
				{
					errors.Add($"Question with id {userQuestionWithAnswers.QuestionId} not found");
					continue;
				}
				if (neededQuestion.QuizQuestionsAnswers.Count != userQuestionWithAnswers.Answers.Count(a => a.WasChosenByUser.HasValue && (bool)a.WasChosenByUser))
				{
					// Если количество ответов пользователя отличается от количества правильных ответов,
					// то получается, что ответ пользователя уже неправильный, переходим к следующему вопросу
					continue;
				}

				int countOfUserCorrectQuestionAnswers = 0;
				foreach (var correctAnswer in neededQuestion.QuizQuestionsAnswers)
				{
					if (neededQuestion.QuizQuestionsAnswers.Count == 1
						&& correctAnswer.AnswerText.ToLowerInvariant()
							.Equals(userQuestionWithAnswers.Answers.Single().UserAnswerText?.ToLowerInvariant() ?? ""))
					{
						countOfUserCorrectQuestions++;
					}
					else if (userQuestionWithAnswers.Answers.Any(a => a.AnswerId == correctAnswer.AnswerId && a.WasChosenByUser.HasValue && (bool)a.WasChosenByUser))
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

				if (countOfUserCorrectQuestionAnswers == neededQuestion.QuizQuestionsAnswers.Count)
				{
					countOfUserCorrectQuestions++;
				}
			}

			if (errors.Count > 0)
			{
				errors.Add("No answers was saved!");
				return new DefaultResponse(errors.ToArray());
			}

			try
			{
				var answeredAt = DateTime.UtcNow;
				if (countOfUserCorrectQuestions == questionsWithCorrectAnswers.Count)
				{
					await SaveUserAnswers(request, questionsWithCorrectAnswers, (int)CourseItemProgressTypesEnumaration.Completed, cancellationToken);
				}
				else
				{
					await SaveUserAnswers(request, questionsWithCorrectAnswers, (int)CourseItemProgressTypesEnumaration.Failed, cancellationToken);
				}

				return new DefaultResponse(null);
			}
			catch (Exception ex)
			{
				return new DefaultResponse([ex.Message]);
			}
		}

		private async Task SaveUserAnswers(
			SetCourseItemQuizUserAnswersRequest request, 
			List<QuizQuestion> questionsWithCorrectAnswers, 
			int courseItemProgressTypeId,
			CancellationToken cancellationToken)
		{
			var userAnswersLogs = new List<QuizQuestionUserAnswerLog>();
			foreach (var userQuestion in request.QuestionsWithUserAnswers)
			{
				var userAnswersByQuestion = userQuestion.Answers.Select(a => new QuizQuestionUserAnswerLog
				{
					QuestionId = userQuestion.QuestionId,
					AnswerId = a.AnswerId,
					UserId = request.UserId,
					IsCorrect = questionsWithCorrectAnswers
						.First(qc => qc.QuestionId == userQuestion.QuestionId)
						.QuizQuestionsAnswers
						.First(ans => ans.AnswerId == a.AnswerId)
						.IsCorrect,
					UserAnswerText = a.UserAnswerText
				}).ToList();
				userAnswersLogs.AddRange(userAnswersByQuestion);
			}

			_dbContext.QuizQuestionUserAnswerLogs.AddRange(userAnswersLogs);
			var courseItemUserProgress = new CourseItemUserProgress
			{
				UserId = request.UserId,
				CourseItemId = request.CourseItemId,
				CourseItemProgressTypeId = courseItemProgressTypeId
			};
			_dbContext.CourseItemUserProgresses.Add(courseItemUserProgress);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
