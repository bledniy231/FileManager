namespace FileManager.Contract.Models
{
	public class DataSetModel
	{
		public long Id { get; set; }
		public long OwnerId { get; set; }
		public DateTime CreatedAt { get; set; }
		public ICollection<BinaryDataModel> Binaries { get; set; }
	}
}
