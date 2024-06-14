namespace PianoMentor.Contract.Files.Upload;

public class UploadCourseItemFileRequest(long userId, int courseItemId, string? contentType, Stream body)
    : BaseUploadFilesRequest(userId, contentType, body)
{
    public int CourseItemId { get; set; } = courseItemId;
}