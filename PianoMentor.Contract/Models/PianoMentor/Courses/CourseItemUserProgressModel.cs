namespace PianoMentor.Contract.Models.PianoMentor.Courses
{
	public class CourseItemUserProgressModel
	{
		public long UserId { get; set; }
		public int CourseId { get; set; }
		public string CourseName { get; set; }
		public int CourseItemId { get; set; }
		public string CourseItemName { get; set; }
		public string CourseItemType { get; set; }
		public string CourseItemProgressType { get; set; }
	}
}
