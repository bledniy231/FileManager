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
using PianoMentor.Contract.Statistics;

namespace PianoMentor.BLL.Statistics
{
    internal class GetUserStatisticsHandler(PianoMentorDbContext dbContext) : IRequestHandler<GetUserStatisticsRequest, GetUserStatisticsResponse>
    {
        public Task<GetUserStatisticsResponse> Handle(GetUserStatisticsRequest request, CancellationToken cancellationToken)
        {
            var coursesUserProgress = dbContext.CourseUserProgresses
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

            var completeCourseItemsCount = dbContext.CourseItemUserProgresses
                .AsNoTracking()
                .Where(ciup =>
                    ciup.UserId == request.UserId
                    && ciup.CourseItemProgressTypeId == (int)CourseItemProgressTypesEnum.Completed)
                .GroupBy(ciup => ciup.CourseItem.CourseItemTypeId)
                .Select(g => new
                {
                    CourseItemTypeId = g.Key,
                    Count = g.Count()
                })
                .ToList();

            var courseItemsCount = dbContext.CourseItems
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
                var quizViewPagerItem = dbContext.ViewPagerTexts
                    .AsNoTracking()
                    .Where(vpt => vpt.Type == "Quiz"
                        && !vpt.IsDeleted
                        && vpt.Id == vpt.ViewPagerTextNumberRanges
                            .OrderByDescending(nr => nr.Number)
                            .First(nr => nr.Number <= quizzesCompletedCount).ViewPagerTextId)
                    .Select(vpt => new ViewPagerTextModel
                    {
                        Title = vpt.Title,
                        Description = GetViewPagerDescription(vpt.Description, quizzesCompletedCount, "Quiz"),
                        ProgressValueAbsolute = quizzesCompletedCount,
                        ProgressValueInPercent = quizzesValueInPercent
                    })
                    .FirstOrDefault();


                var coursesValueAbsolute = coursesUserProgress.Count(cup => cup.ProgressInPercent == 100);

                var courseViewPagerItem = dbContext.ViewPagerTexts
                    .AsNoTracking()
                    .Where(vpt => vpt.Type == "Course"
                        && !vpt.IsDeleted
                        && vpt.Id == vpt.ViewPagerTextNumberRanges
                            .OrderByDescending(nr => nr.Number)
                            .First(nr => nr.Number <= coursesValueAbsolute).ViewPagerTextId)
                    .Select(vpt => new ViewPagerTextModel
                    {
                        Title = vpt.Title,
                        Description = GetViewPagerDescription(vpt.Description, coursesValueAbsolute, "Course"),
                        ProgressValueAbsolute = coursesValueAbsolute,
                        ProgressValueInPercent = GetPercentValue(coursesValueAbsolute, coursesUserProgress.Count)
                    })
                    .FirstOrDefault();


                ViewPagerTextModel[] viewPagerList = [quizViewPagerItem, courseViewPagerItem];

                var lectureStatistics = new BaseStatisticsModel
                {
                    ProgressValueAbsolute = lecturesCompletedCount,
                    ProgressValueInPercent = (int)Math.Round((double)lecturesCompletedCount / courseItemsCount.First(IsLecture).Count * 100),
                    Title = WordsEndingsManager.GetSimpleEnding(CourseItemTypesEnum.Lecture, lecturesCompletedCount)
                };

                var exerciseStatistics = new BaseStatisticsModel
                {
                    ProgressValueAbsolute = exercisesCompletedCount,
                    ProgressValueInPercent = (int)Math.Round((double)exercisesCompletedCount / courseItemsCount.First(IsExercise).Count * 100),
                    Title = WordsEndingsManager.GetSimpleEnding(CourseItemTypesEnum.Exercise, exercisesCompletedCount)
                };

                var quizStatistics = new BaseStatisticsModel
                {
                    ProgressValueAbsolute = quizzesCompletedCount,
                    ProgressValueInPercent = (int)Math.Round((double)quizzesCompletedCount / courseItemsCount.First(IsQuiz).Count * 100),
                    Title = WordsEndingsManager.GetSimpleEnding(CourseItemTypesEnum.Quiz, quizzesCompletedCount)
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
            catch (Exception ex)
            {
                return Task.FromResult(new GetUserStatisticsResponse(null, null, null, null, null, [ex.Message]));
            }
        }

        private static bool IsLecture(dynamic anonObject) => anonObject.CourseItemTypeId == (int)CourseItemTypesEnum.Lecture;
        private static bool IsExercise(dynamic anonObject) => anonObject.CourseItemTypeId == (int)CourseItemTypesEnum.Exercise;
        private static bool IsQuiz(dynamic anonObject) => anonObject.CourseItemTypeId == (int)CourseItemTypesEnum.Quiz;

        private static int GetPercentValue(int completedCount, int totalCount)
            => (int)Math.Round((double)completedCount / totalCount * 100);

        private static string GetViewPagerDescription(string descriptionWithPlaceholders, int count, string type)
        {
            string ending = WordsEndingsManager.GetEndingForTextWithPlaceholders(type, count)
                ?? throw new ArgumentNullException("Wrong item type for creting view pager description");

            return descriptionWithPlaceholders.Replace("<number>", count.ToString()).Replace("<subject>", ending);
        }
    }
}
