using FileManager.Contract.ApplicationUser;
using FileManager.DAL.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FileManager.BLL.ApplicationUser
{
	internal class RevokeUsersInRoleHandler(
		UserManager<FileManagerUser> userManager) 
		: IRequestHandler<RevokeUsersInRoleRequest, Unit>
	{
		private readonly UserManager<FileManagerUser> _userManager = userManager;

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
