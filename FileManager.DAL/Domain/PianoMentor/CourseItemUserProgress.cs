using FileManager.DAL.Domain.Identity;

namespace FileManager.DAL.Domain.PianoMentor
{
	public class CourseItemUserProgress
	{
		public long Id { get; set; }
		public long UserId { get; set; }
		public int CourseItemId { get; set; }
		public int CourseItemProgressTypeId { get; set; }
		public FileManagerUser User { get; set; }
		public CourseItem CourseItem { get; set; }
		public CourseItemProgressType CourseItemProgressType { get; set; }
	}
}
