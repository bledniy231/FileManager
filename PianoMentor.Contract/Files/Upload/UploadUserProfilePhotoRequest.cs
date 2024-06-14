namespace PianoMentor.Contract.Files.Upload;

public class UploadUserProfilePhotoRequest(long userId, string? contentType, Stream body)
    : BaseUploadFilesRequest(userId, contentType, body) { }