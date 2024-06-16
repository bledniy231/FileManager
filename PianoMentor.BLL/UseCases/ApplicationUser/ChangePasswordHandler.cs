using MediatR;
using Microsoft.AspNetCore.Identity;
using PianoMentor.Contract.ApplicationUser;
using PianoMentor.Contract.Default;
using PianoMentor.DAL.Models.Identity;

namespace PianoMentor.BLL.UseCases.ApplicationUser;

public class ChangePasswordHandler(UserManager<PianoMentorUser> userManager) : IRequestHandler<ChangePasswordRequest, DefaultResponse>
{
    public async Task<DefaultResponse> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(request.User);
        if (user == null)
        {
            return new DefaultResponse(["User not found"]);
        }

        if (request.NewPassword != request.RepeatNewPassword)
        {
            return new DefaultResponse(["New password and repeated new password do not match"]);
        }

        var changePasswordResult = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!changePasswordResult.Succeeded)
        {
            return new DefaultResponse(changePasswordResult.Errors.Select(e => e.Description).ToArray());
        }

        return new DefaultResponse();
    }
}