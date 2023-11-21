namespace FileManager.DAL.Domain.DataSet
{
	public class Storage
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string BasePath { get; set; }
		public bool AllowWrite { get; set; }
	}
}
