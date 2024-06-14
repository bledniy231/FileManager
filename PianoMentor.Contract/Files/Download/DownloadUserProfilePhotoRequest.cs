using MediatR;

namespace PianoMentor.Contract.Files.Download;

public class DownloadUserProfilePhotoRequest(long userId) : IRequest<DownloadFilesResponse>
{
    public long UserId { get; set; } = userId;
}