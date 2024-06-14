namespace PianoMentor.DAL.Models.PianoMentor.Texts
{
	public class ViewPagerText
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public bool IsDeleted { get; set; }
		public string Type { get; set; }
		public List<ViewPagerTextNumberRanges> ViewPagerTextNumberRanges { get; set; }
	}
}
