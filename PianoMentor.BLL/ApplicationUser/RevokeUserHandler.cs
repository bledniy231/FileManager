using PianoMentor.Contract.ApplicationUser;
using PianoMentor.Contract.Default;
using PianoMentor.DAL.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace PianoMentor.BLL.ApplicationUser
{
	internal class RevokeUserHandler(
		UserManager<PianoMentorUser> userManager) 
		: IRequestHandler<RevokeUserRequest, DefaultResponse>
	{
		private readonly UserManager<PianoMentorUser> _userManager = userManager;

		public async Task<DefaultResponse> Handle(RevokeUserRequest request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByNameAsync(request.Username);
			if (user == null)
			{
				return new DefaultResponse([$"Cannot find user with username {request.Username}"]);
			}

			user.RefreshToken = null;
			await _userManager.UpdateAsync(user);

			return new DefaultResponse(null);
		}
	}
}
