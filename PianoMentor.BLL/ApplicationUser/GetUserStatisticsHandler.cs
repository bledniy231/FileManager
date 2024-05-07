using PianoMentor.Contract.ApplicationUser;
using PianoMentor.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PianoMentor.Contract.Models.PianoMentor.Courses;
using Microsoft.AspNetCore.Components.Web;
using PianoMentor.Contract.Models.PianoMentor.Statistics;
using PianoMentor.BLL.WordsEndings;
using PianoMentor.Contract.Models.PianoMentor.Texts;
using PianoMentor.DAL.Models.PianoMentor.Texts;
using System.Net;

namespace PianoMentor.BLL.ApplicationUser
{
	internal class GetUserStatisticsHandler(
		PianoMentorDbContext dbContext,
		WordsEndingsManager wordsEndingsManager) : IRequestHandler<GetUserStatisticsRequest, GetUserStatisticsResponse>
	{
		private readonly PianoMentorDbContext _dbContext = dbContext;
		private readonly WordsEndingsManager _wordsEndingsManager = wordsEndingsManager;

		public Task<GetUserStatisticsResponse> Handle(GetUserStatisticsRequest request, CancellationToken cancellationToken)
		{
			var coursesUserProgress = _dbContext.CourseUserProgresses
				.AsNoTracking()
				.Where(cup => cup.UserId == request.UserId)
				.Select(cup => new CourseUserProgressModel
				{
					CourseId = cup.CourseId,
					CourseName = cup.Course.Title,
					ProgressInPercent = cup.ProgressInPercent,
					UserId = request.UserId
				})
				.ToList();

			var completeCourseItemsCount = _dbContext.CourseItemUserProgresses
				.AsNoTracking()
				.Where(ciup =>
					ciup.UserId == request.UserId
					&& ciup.CourseItemProgressTypeId == (int)CourseItemProgressTypesEnumaration.Completed)
				.GroupBy(ciup => ciup.CourseItem.CourseItemTypeId)
				.Select(g => new
				{
					CourseItemTypeId = g.Key,
					Count = g.Count()
				})
				.ToList();

			var courseItemsCount = _dbContext.CourseItems
				.AsNoTracking()
				.GroupBy(ci => ci.CourseItemTypeId)
				.Select(g => new
				{
					CourseItemTypeId = g.Key,
					Count = g.Count()
				})
				.ToList();

			int lecturesCompletedCount = completeCourseItemsCount.FirstOrDefault(IsLecture)?.Count ?? 0;
			int exercisesCompletedCount = completeCourseItemsCount.FirstOrDefault(IsExercise)?.Count ?? 0;
			int quizzesCompletedCount = completeCourseItemsCount.FirstOrDefault(IsQuiz)?.Count ?? 0;
			int lecturesValueInPercent = GetPercentValue(lecturesCompletedCount, courseItemsCount.First(IsQuiz).Count);
			int exercisesValueInPercent = GetPercentValue(exercisesCompletedCount, courseItemsCount.First(IsExercise).Count);
			int quizzesValueInPercent = GetPercentValue(quizzesCompletedCount, courseItemsCount.First(IsQuiz).Count);

			try
			{
				var quizViewPagerItem = _dbContext.ViewPagerTexts
					.AsNoTracking()
					.Where(vpt => vpt.Type == "Quiz"
								&& !vpt.IsDeleted
								&& vpt.ViewPagerTextNumberRanges
									.Any(nr => nr.Number <= quizzesCompletedCount))
									.OrderByDescending(vpt => vpt.ViewPagerTextNumberRanges.Max(nr => nr.Number))
					.Select(vpt => new ViewPagerTextModel
					{
						Title = vpt.Title,
						Description = vpt.Description,
						ProgressValueAbsolute = quizzesCompletedCount,
						ProgressValueInPercent = quizzesValueInPercent
					})
					.FirstOrDefault();


				var coursesValueAbsolute = coursesUserProgress.Count(cup => cup.ProgressInPercent == 100);

				var courseViewPagerItem = _dbContext.ViewPagerTexts
					.AsNoTracking()
					.Where(vpt => vpt.Type == "Course"
								&& !vpt.IsDeleted
								&& vpt.ViewPagerTextNumberRanges
									.Any(nr => nr.Number <= coursesUserProgress.Count))
									.OrderByDescending(vpt => vpt.ViewPagerTextNumberRanges.Max(nr => nr.Number))
					.Select(vpt => new ViewPagerTextModel
					{
						Title = vpt.Title,
						Description = vpt.Description,
						ProgressValueAbsolute = coursesValueAbsolute,
						ProgressValueInPercent = GetPercentValue(coursesValueAbsolute, coursesUserProgress.Count)
					})
					.FirstOrDefault();


				ViewPagerTextModel[] viewPagerList = [quizViewPagerItem, courseViewPagerItem];

				var lectureStatistics = new BaseStatisticsModel
				{
					ProgressValueAbsolute = lecturesCompletedCount,
					ProgressValueInPercent = (int)Math.Round((double)lecturesCompletedCount / courseItemsCount.First(IsLecture).Count * 100),
					Title = _wordsEndingsManager.GetEnding(CourseItemTypesEnumeration.Lecture, lecturesCompletedCount)
				};

				var exerciseStatistics = new BaseStatisticsModel
				{
					ProgressValueAbsolute = exercisesCompletedCount,
					ProgressValueInPercent = (int)Math.Round((double)exercisesCompletedCount / courseItemsCount.First(IsExercise).Count * 100),
					Title = _wordsEndingsManager.GetEnding(CourseItemTypesEnumeration.Exercise, exercisesCompletedCount)
				};

				var quizStatistics = new BaseStatisticsModel
				{
					ProgressValueAbsolute = quizzesCompletedCount,
					ProgressValueInPercent = (int)Math.Round((double)quizzesCompletedCount / courseItemsCount.First(IsQuiz).Count * 100),
					Title = _wordsEndingsManager.GetEnding(CourseItemTypesEnumeration.Quiz, quizzesCompletedCount)
				};

				var currentCourse = coursesUserProgress.FirstOrDefault(cup => cup.ProgressInPercent != 100) ?? new CourseUserProgressModel() { CourseName = "Курс \"Введение\"", ProgressInPercent = 0 };
				var courseStatistics = new BaseStatisticsModel
				{
					ProgressValueAbsolute = currentCourse.ProgressInPercent,
					ProgressValueInPercent = currentCourse.ProgressInPercent,
					Title = currentCourse.CourseName
				};

				return Task.FromResult(new GetUserStatisticsResponse(lectureStatistics, exerciseStatistics, quizStatistics, courseStatistics, viewPagerList, null));
			}
			catch
			{
				return Task.FromResult(new GetUserStatisticsResponse(null, null, null, null, null, ["No courses items"]));
			}
		}

		private static bool IsLecture(dynamic anonObject) => anonObject.CourseItemTypeId == (int)CourseItemTypesEnumeration.Lecture;
		private static bool IsExercise(dynamic anonObject) => anonObject.CourseItemTypeId == (int)CourseItemTypesEnumeration.Exercise;
		private static bool IsQuiz(dynamic anonObject) => anonObject.CourseItemTypeId == (int)CourseItemTypesEnumeration.Quiz;

		private static bool IsCorrectViewPagerText(ViewPagerText vpt, string itemType, int itemCount)
			=> vpt.Type == itemType
				&& !vpt.IsDeleted
				&& vpt.Id == (vpt.ViewPagerTextNumberRanges
					.LastOrDefault(nr =>
						nr.ViewPagerTextId == vpt.Id
						&& nr.Number <= itemCount)?.ViewPagerTextId ?? 0);

		private static int GetPercentValue(int completedCount, int totalCount)
			=> (int)Math.Round((double)completedCount / totalCount * 100);
	}
}
