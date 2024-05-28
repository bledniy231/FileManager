using MediatR;
using PianoMentor.Contract.Default;

namespace PianoMentor.Contract.Files;

public class UploadUserProfilePhotoRequest(long userId, string? contentType, Stream stream) : IRequest<DefaultResponse>
{
    public long UserId { get; set; } = userId;  
    public string? ContentType { get; set; } = contentType;
    public Stream Stream { get; set; } = stream;
}