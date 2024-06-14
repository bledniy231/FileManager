using MediatR;
using Microsoft.AspNetCore.Identity;
using PianoMentor.Contract.ApplicationUser;
using PianoMentor.DAL.Models.Identity;

namespace PianoMentor.BLL.UseCases.ApplicationUser
{
	internal class RegisterUserHandler(
		UserManager<PianoMentorUser> userManager,
		RoleManager<IdentityRole<long>> roleManager) 
		: IRequestHandler<RegisterUserRequest, RegisterUserResponse>
	{
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

			var identityResult = await userManager.CreateAsync(user, request.Password);

			if (!identityResult.Succeeded)
			{
				return new RegisterUserResponse
				{
					Errors = identityResult.Errors.Select(er => er.Description).ToArray()
				};
			}

			foreach (var roleName in request.Roles)
			{
				if (await roleManager.RoleExistsAsync(roleName))
				{
					await userManager.AddToRoleAsync(user, roleName);
				}
				else
				{
					await userManager.DeleteAsync(user);

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
