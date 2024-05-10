using MediatR;

namespace PianoMentor.Contract.Files
{
	public class DownloadFilesRequest(long dataSetId, int? dataId, bool isZipArchiveResponse = true) : IRequest<DownloadFilesResponse>
	{
		public long DataSetId { get; set; } = dataSetId;
		/// <summary>
		/// Будет null, если пользователь хочет скачать все файлы в data set
		/// </summary>
		public int? DataId { get; set; } = dataId;

		public bool IsZipArchiveResponse { get; set; } = isZipArchiveResponse;
	}
}
