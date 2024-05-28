using MediatR;
using PianoMentor.Contract.Default;

namespace PianoMentor.Contract.Files
{
	public class UploadQuestionImageRequest(long userId, int questionId, string? contentType, Stream body) : IRequest<DefaultResponse>
	{
		public long UserId { get; set; } = userId;
		public int QuestionId { get; set; } = questionId;
		public string? ContentType { get; set; } = contentType;
		public Stream Body { get; set; } = body;
	}
}
