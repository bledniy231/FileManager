using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PianoMentor.BLL.Services.TokenService;
using PianoMentor.Contract.ApplicationUser;
using PianoMentor.DAL.Models.Identity;

namespace PianoMentor.BLL.UseCases.ApplicationUser
{
	internal class AuthUserHandler(
		UserManager<PianoMentorUser> userManager,
		ITokenService tokenService,
		IConfiguration config) 
		: IRequestHandler<AuthUserRequest, AuthUserResponse>
	{
		public async Task<AuthUserResponse> Handle(AuthUserRequest request, CancellationToken cancellationToken)
		{
			var managedUser = await userManager.FindByEmailAsync(request.Email);
			if (managedUser == null)
			{
				return new AuthUserResponse
				{
					FailedMessage = $"No accounts registered with {request.Email}"
				};
			}

			if (!await userManager.CheckPasswordAsync(managedUser, request.Password))
			{
				return new AuthUserResponse
				{
					FailedMessage = $"Incorrect password for {request.Email}"
				};
			}

			var roles = await userManager.GetRolesAsync(managedUser);

			var (accessToken, accessTokenExpiryDateTime) = tokenService.CreateAccessToken(managedUser, roles);
			managedUser.RefreshToken = tokenService.CreateRefreshToken();
			managedUser.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(config.GetSection("Jwt:RefreshTokenValidityInDays").Get<int>());

			var updatingResult = await userManager.UpdateAsync(managedUser);

			if (!updatingResult.Succeeded)
			{
				return new AuthUserResponse
				{
					FailedMessage = $"Cannot update user information for {request.Email}"
				};
			}


			return new AuthUserResponse
			{
				JwtTokensModel = new Contract.Models.JwtTokens.JwtTokensModel
				{
					AccessToken = accessToken,
					AccessTokenExpireTime = accessTokenExpiryDateTime,
					RefreshToken = managedUser.RefreshToken,
					RefreshTokenExpireTime = managedUser.RefreshTokenExpireTime
				},
				UserName = managedUser.UserName,
				Roles = roles,
				UserId = managedUser.Id,
				Email = request.Email
			};
		}
	}
}
