using System.Security.Claims;
using MediatR;
using PianoMentor.Contract.Default;

namespace PianoMentor.Contract.ApplicationUser;

public class ChangePasswordRequest : IRequest<DefaultResponse>
{
    public ClaimsPrincipal User { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string RepeatNewPassword { get; set; }
}