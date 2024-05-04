using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.PianoMentor.Courses;
using System.ComponentModel;
using System.Reflection;

namespace PianoMentor.Contract.ApplicationUser
{
	public class GetUserStatisticsResponse(
		List<CourseUserProgressModel> coursesUsersProgress, 
		int completedLectures,
		int allLectures,
		int completedExercises,
		int allExcercises,
		int completedQuizzes,
		int allQuizzes,
		string[]? errors) : DefaultResponse(errors)
	{
		public List<CourseUserProgressModel> CoursesUserProgress { get; set; } = coursesUsersProgress;
		public int CompletedLectures { get; set; } = completedLectures;
		public int AllLectures { get; set; } = allLectures;
		public int CompletedExercises { get; set; } = completedExercises;
		public int AllExcercises { get; set; } = allExcercises;
		public int CompletedQuizzes { get; set; } = completedQuizzes;
		public int AllQuizzes { get; set; } = allQuizzes;
	}
}