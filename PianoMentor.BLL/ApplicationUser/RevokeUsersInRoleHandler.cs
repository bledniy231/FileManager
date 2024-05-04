using PianoMentor.Contract.ApplicationUser;
using PianoMentor.DAL.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PianoMentor.BLL.ApplicationUser
{
	internal class RevokeUsersInRoleHandler(
		UserManager<PianoMentorUser> userManager) 
		: IRequestHandler<RevokeUsersInRoleRequest, Unit>
	{
		private readonly UserManager<PianoMentorUser> _userManager = userManager;

		public async Task<Unit> Handle(RevokeUsersInRoleRequest request, CancellationToken cancellationToken)
		{
			var users = request.Role switch
			{
				not null => await _userManager.GetUsersInRoleAsync(request.Role),
				_ => await _userManager.Users.ToListAsync(cancellationToken)
			};

			foreach (var user in users)
			{
				user.RefreshToken = null;
				await _userManager.UpdateAsync(user);
			}

			return Unit.Value;
		}
	}
}
