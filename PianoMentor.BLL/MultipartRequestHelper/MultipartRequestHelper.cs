using Microsoft.Net.Http.Headers;

namespace PianoMentor.BLL.MultipartRequestHelper
{
	public class MultipartRequestHelper : IMultipartRequestHelper
	{
		public string? GetBoundary(MediaTypeHeaderValue headerValue, int lengthLimit)
		{
			var boundary = HeaderUtilities.RemoveQuotes(headerValue.Boundary).Value;

			return (string.IsNullOrWhiteSpace(boundary) || boundary.Length > lengthLimit) ? null : boundary;
		}

		public bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
			=> contentDisposition != null 
			&& contentDisposition.DispositionType.Equals("form-data") 
			&& (!string.IsNullOrEmpty(contentDisposition.FileName.Value) 
				|| !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));

		public bool IsMultipartContentType(string contentType)
			=> !string.IsNullOrEmpty(contentType) && contentType.Contains("multipart/", StringComparison.OrdinalIgnoreCase);
	}
}
