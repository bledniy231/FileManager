using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.PianoMentor.Courses;

namespace PianoMentor.Contract.ApplicationUser
{
	public class GetUserStatisticsResponse(
		List<CourseUserProgressModel> coursesUsersProgress, 
		int completedLectures,
		int completedExercises,
		int completedQuizzes,
		string[]? errors) : DefaultResponse(errors)
	{
		public List<CourseUserProgressModel> CoursesUserProgress { get; set; } = coursesUsersProgress;
		public int CompletedLectures { get; set; } = completedLectures;
		public int CompletedExercises { get; set; } = completedExercises;
		public int CompletedQuizzes { get; set; } = completedQuizzes;
	}
}