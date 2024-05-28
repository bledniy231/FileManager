using PianoMentor.Contract.ApplicationUser;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PianoMentor.DAL.Models.Identity;

namespace PianoMentor.BLL.ApplicationUser
{
	internal class RevokeUsersInRoleHandler(
		UserManager<PianoMentorUser> userManager) 
		: IRequestHandler<RevokeUsersInRoleRequest, Unit>
	{
		public async Task<Unit> Handle(RevokeUsersInRoleRequest request, CancellationToken cancellationToken)
		{
			var users = request.Role switch
			{
				not null => await userManager.GetUsersInRoleAsync(request.Role),
				_ => await userManager.Users.ToListAsync(cancellationToken)
			};

			foreach (var user in users)
			{
				user.RefreshToken = null;
				await userManager.UpdateAsync(user);
			}

			return Unit.Value;
		}
	}
}
