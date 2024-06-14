namespace PianoMentor.DAL.Models.DataSet
{
	public class BinaryData
	{
		public long DataSetId { get; set; }
		public int DataId { get; set; }
		public string Filename { get; set; }
		public long Length { get; set; }
		public int BinaryTypeId { get; set; }
		public bool IsDeleted { get; set; }
		public BinaryType BinaryType { get; set; }
		public DataSet DataSet { get; set; }
	}
}
