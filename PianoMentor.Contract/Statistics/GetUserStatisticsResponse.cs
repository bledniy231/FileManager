using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.PianoMentor.Courses;
using PianoMentor.Contract.Models.PianoMentor.Statistics;
using PianoMentor.Contract.Models.PianoMentor.Texts;

namespace PianoMentor.Contract.Statistics
{
    public class GetUserStatisticsResponse(
        BaseStatisticsModel lectureStatistics,
        BaseStatisticsModel exerciseStatistics,
        BaseStatisticsModel quizStatistics,
        BaseStatisticsModel currentCourseStatistics,
        ViewPagerTextModel[] viewPagerTexts,
        string[]? errors) : DefaultResponse(errors)
    {
        public BaseStatisticsModel LectureStatistics { get; set; } = lectureStatistics;
        public BaseStatisticsModel ExerciseStatistics { get; set; } = exerciseStatistics;
        public BaseStatisticsModel QuizStatistics { get; set; } = quizStatistics;
        /// <summary>
        /// Это именно модель прогресса курса, который является первым после последнего завершенного курса
        /// В случае, если начато несколько курсов, но ни один не завершен, то в модели будет представлен самый первый курс
        /// </summary>
        public BaseStatisticsModel CourseStatistics { get; set; } = currentCourseStatistics;
        public ViewPagerTextModel[] ViewPagerTexts { get; set; } = viewPagerTexts;
    }
}