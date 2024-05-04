namespace PianoMentor.DAL.Domain.DataSet
{
	public class BinaryData
	{
		public long DataSetId { get; set; }
		public int DataId { get; set; }
		public string Filename { get; set; }
		public long Length { get; set; }
		public DataSet DataSet { get; set; }
	}
}
