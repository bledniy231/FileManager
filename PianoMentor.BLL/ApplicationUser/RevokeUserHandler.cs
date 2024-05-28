using PianoMentor.Contract.ApplicationUser;
using PianoMentor.Contract.Default;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PianoMentor.DAL.Models.Identity;

namespace PianoMentor.BLL.ApplicationUser
{
	internal class RevokeUserHandler(
		UserManager<PianoMentorUser> userManager) 
		: IRequestHandler<RevokeUserRequest, DefaultResponse>
	{
		public async Task<DefaultResponse> Handle(RevokeUserRequest request, CancellationToken cancellationToken)
		{
			var user = await userManager.FindByNameAsync(request.Username);
			if (user == null)
			{
				return new DefaultResponse([$"Cannot find user with username {request.Username}"]);
			}

			user.RefreshToken = null;
			await userManager.UpdateAsync(user);

			return new DefaultResponse(null);
		}
	}
}
