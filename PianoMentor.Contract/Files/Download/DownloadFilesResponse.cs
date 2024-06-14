using PianoMentor.Contract.Default;

namespace PianoMentor.Contract.Files.Download
{
	public class DownloadFilesResponse(
		FileStream fileStream,
		string contentType,
		string fileDownloadName,
		string[]? errors) : DefaultResponse(errors)
	{
		public FileStream FileStream { get; set; } = fileStream;
		public string ContentType { get; set; } = contentType;
		public string FileDownloadName { get; set; } = fileDownloadName;
	}
}
