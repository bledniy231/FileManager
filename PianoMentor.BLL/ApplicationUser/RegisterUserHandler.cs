using PianoMentor.Contract.ApplicationUser;
using PianoMentor.DAL.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace PianoMentor.BLL.ApplicationUser
{
	internal class RegisterUserHandler(
		UserManager<PianoMentorUser> userManager,
		RoleManager<IdentityRole<long>> roleManager) 
		: IRequestHandler<RegisterUserRequest, RegisterUserResponse>
	{
		private readonly UserManager<PianoMentorUser> _userManager = userManager;
		private readonly RoleManager<IdentityRole<long>> _roleManager = roleManager;

		public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
		{
			if (!request.Password.Equals(request.PasswordConfirm))
			{
				return new RegisterUserResponse
				{
					Errors = [$"Connot confirm password for {request.Email}"]
				};
			}

			var user = new PianoMentorUser
			{
				Email = request.Email,
				UserName = request.UserName
			};

			var identityResult = await _userManager.CreateAsync(user, request.Password);

			if (!identityResult.Succeeded)
			{
				return new RegisterUserResponse
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

					return new RegisterUserResponse
					{
						Errors = [$"Role {roleName} does not exists, registration failed"]
					};
				}
			}

			return new RegisterUserResponse
			{
				Email = request.Email,
				Password = request.Password
			};
		}
	}
}
