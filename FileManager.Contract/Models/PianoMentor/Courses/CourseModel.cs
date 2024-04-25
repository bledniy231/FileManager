namespace FileManager.Contract.Models.PianoMentor.Courses
{
	public class CourseModel
	{
		public int CourseId { get; set; }
		public int Position { get; set; }
		public string Title { get; set; }
		public string Subtitle { get; set; }
		public string Description { get; set; }
		public int ProgressInPercent { get; set; }
		public List<CourseItemModel> CourseItemModels { get; set; }
	}
}
