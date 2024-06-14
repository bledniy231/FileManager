using MediatR;

namespace PianoMentor.Contract.Files.Upload;

public class BaseUploadFilesRequest(long userId, string? contentType, Stream body) : IRequest<UploadFilesResponse>
{
    public long UserId { get; set; } = userId;  
    public string? ContentType { get; set; } = contentType;
    public Stream Body { get; set; } = body;
}