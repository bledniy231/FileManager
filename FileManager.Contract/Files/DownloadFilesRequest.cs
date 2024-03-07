using MediatR;

namespace FileManager.Contract.Files
{
	public class DownloadFilesRequest(long dataSetId, int? dataId) : IRequest<DownloadFilesResponse>
	{
		public long DataSetId { get; set; } = dataSetId;
		/// <summary>
		/// Будет null, если пользователь хочет скачать все файлы в data set
		/// </summary>
		public int? DataId { get; set; } = dataId;
	}
}
