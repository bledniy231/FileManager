using MediatR;

namespace PianoMentor.Contract.Files;

public class GetUserProfilePhotoRequest(long userId) : IRequest<DownloadFilesResponse>
{
    public long UserId { get; set; } = userId;
}