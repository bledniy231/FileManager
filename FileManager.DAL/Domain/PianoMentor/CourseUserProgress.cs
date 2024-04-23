using FileManager.DAL.Domain.Identity;

namespace FileManager.DAL.Domain.PianoMentor
{
	public class CourseUserProgress
	{
		public long Id { get; set; }
		public long UserId { get; set; }
		public int CourseId { get; set; }
		public uint ProgressInPercent { get; set; }
		public FileManagerUser User { get; set; }
		public Course Course { get; set; }
	}
}
