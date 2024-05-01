using FileManager.Contract.Default;
using MediatR;

namespace FileManager.Contract.Files
{
	public class UploadFilesRequest(long userId, int? courseItemId, string? contentType, Stream body) : IRequest<DefaultResponse>
	{
		public long UserId { get; set; } = userId;
		public int? CourseItemId { get; set; } = courseItemId;
		public string? ContentType { get; set; } = contentType;
		public Stream Body { get; set; } = body;
	}
}
