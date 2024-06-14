namespace PianoMentor.Contract.Files.Upload;

public class UploadQuestionImageRequest(long userId, int questionId, string? contentType, Stream body)
	: BaseUploadFilesRequest(userId, contentType, body)
{
	public int QuestionId { get; set; } = questionId;
}
