using MediatR;
using PianoMentor.Contract.Files;

namespace PianoMentor.BLL.Files;

public class GetUserProfilePhotoHandler : IRequestHandler<GetUserProfilePhotoRequest, DownloadFilesResponse>
{
    public Task<DownloadFilesResponse> Handle(GetUserProfilePhotoRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}