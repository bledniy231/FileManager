namespace FileManager.Contract.Models.PianoMentor.Courses
{
	public class CourseItemModel
	{
		public int CourseItemId { get; set; }
		public int Position { get; set; }
		public string Title { get; set; }
		public string CourseItemType { get; set; }
		public string? CourseItemProgressType { get; set; }
		public int CourseId { get; set; }
	}
}
