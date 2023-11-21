using Microsoft.Net.Http.Headers;

namespace FileManager.BLL.MultipartRequestHelper
{
	public interface IMultipartRequestHelper
	{
		public bool IsMultipartContentType(string contentType);
		public string? GetBoundary(MediaTypeHeaderValue headerValue, int lengthLimit);
		public bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition);
	}
}
