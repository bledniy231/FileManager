using FileManager.Contract.ApplicationUser;
using FileManager.DAL.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FileManager.BLL.ApplicationUser
{
	internal class UserRegisterHandler(
		UserManager<FileManagerUser> userManager,
		RoleManager<IdentityRole<long>> roleManager) 
		: IRequestHandler<UserRegisterRequest, UserRegisterResponse>
	{
		private readonly UserManager<FileManagerUser> _userManager = userManager;
		private readonly RoleManager<IdentityRole<long>> _roleManager = roleManager;

		public async Task<UserRegisterResponse> Handle(UserRegisterRequest request, CancellationToken cancellationToken)
		{
			if (!request.Password.Equals(request.PasswordConfirm))
			{
				return new UserRegisterResponse
				{
					Errors = [$"Connot confirm password for {request.Email}"]
				};
			}

			var user = new FileManagerUser
			{
				Email = request.Email,
				UserName = request.UserName
			};

			var identityResult = await _userManager.CreateAsync(user, request.Password);

			if (!identityResult.Succeeded)
			{
				return new UserRegisterResponse
				{
					Errors = identityResult.Errors.Select(er => er.Description).ToArray()
				};
			}

			foreach (var roleName in request.Roles)
			{
				if (await _roleManager.RoleExistsAsync(roleName))
				{
					await _userManager.AddToRoleAsync(user, roleName);
				}
				else
				{
					await _userManager.DeleteAsync(user);

					return new UserRegisterResponse
					{
						Errors = [$"Role {roleName} does not exists, registration failed"]
					};
				}
			}

			return new UserRegisterResponse
			{
				Email = request.Email,
				Password = request.Password
			};
		}
	}
}
