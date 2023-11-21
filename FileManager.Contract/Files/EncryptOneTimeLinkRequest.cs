using MediatR;

namespace FileManager.Contract.Files
{
	public class EncryptOneTimeLinkRequest(long dataSetId, int? dataId) : IRequest<EncryptOneTimeLinkResponse>
	{
		public long DataSetId { get; set; } = dataSetId;
		public int? DataId { get; set; } = dataId;
	}
}
