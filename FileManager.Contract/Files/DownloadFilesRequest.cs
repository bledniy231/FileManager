using MediatR;

namespace FileManager.Contract.Files
{
	public class DownloadFilesRequest(long dataSetId, int? dataId) : IRequest<DownloadFilesResponse>
	{
		public long DataSetId { get; set; } = dataSetId;
		public int? DataId { get; set; } = dataId;
	}
}
