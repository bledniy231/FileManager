using FileManager.Contract.Default;

namespace FileManager.Contract.Files
{
	public class DecryptOneTimeLinkResponse(
		long dataSetId,
		int? dataId,
		string[]? errors) : DefaultResponse(errors)
	{
		public long DataSetId { get; set; } = dataSetId;
		public int? DataId { get; set; } = dataId;
	}
}
