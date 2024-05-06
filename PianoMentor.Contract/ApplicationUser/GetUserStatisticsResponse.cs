using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.PianoMentor.Courses;
using PianoMentor.Contract.Models.PianoMentor.Statistics;
using PianoMentor.Contract.Models.PianoMentor.Texts;

namespace PianoMentor.Contract.ApplicationUser
{
	public class GetUserStatisticsResponse(
		List<CourseUserProgressModel> coursesUsersProgress, 
		BaseStatisticsModel lectureStatistics,
		BaseStatisticsModel exersiceStatistics,
		BaseStatisticsModel quizStatistics,
		ViewPagerTextModel[] viewPagerTexts,
		string[]? errors) : DefaultResponse(errors)
	{
		public List<CourseUserProgressModel> CoursesUserProgress { get; set; } = coursesUsersProgress;
		public BaseStatisticsModel LectureStatistics { get; set; } = lectureStatistics;
		public BaseStatisticsModel ExersiceStatistics { get; set; } = exersiceStatistics;
		public BaseStatisticsModel QuizStatistics { get; set; } = quizStatistics;
		public ViewPagerTextModel[] ViewPagerTexts { get; set; } = viewPagerTexts;
	}
}