namespace FileManager.Contract.Files
{
	public class DownloadFilesResponse
	{
		public FileStream FileStream { get; set; }
		public string ContentType { get; set; }
		public string FileDownloadName { get; set; }

		public bool IsSuccess { get; set; }
		public string? Message { get; set; }
	}
}
