namespace PianoMentor.Contract.Files.Upload;

public class UploadUsersFilesRequest(long userId, string? contentType, Stream body) 
	: BaseUploadFilesRequest(userId, contentType, body) { }
