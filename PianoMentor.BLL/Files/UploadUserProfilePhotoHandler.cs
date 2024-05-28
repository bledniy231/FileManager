using MediatR;
using PianoMentor.Contract.Default;
using PianoMentor.Contract.Files;

namespace PianoMentor.BLL.Files;

public class UploadUserProfilePhotoHandler : IRequestHandler<UploadUserProfilePhotoRequest, DefaultResponse>
{
    public Task<DefaultResponse> Handle(UploadUserProfilePhotoRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}