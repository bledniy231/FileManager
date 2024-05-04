namespace PianoMentor.Contract.Models.PianoMentor.Courses
{
	public class CourseUserProgressModel
	{
		public long UserId { get; set; }
		public int CourseId { get; set; }
		public string CourseName { get; set; }
		public int ProgressInPercent { get; set; }
	}
}
