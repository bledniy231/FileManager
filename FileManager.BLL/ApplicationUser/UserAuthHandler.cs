using FileManager.BLL.TokenService;
using FileManager.Contract.ApplicationUser;
using FileManager.DAL.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace FileManager.BLL.ApplicationUser
{
	internal class UserAuthHandler(
		UserManager<FileManagerUser> userManager,
		ITokenService tokenService,
		IConfiguration config) 
		: IRequestHandler<UserAuthRequest, UserAuthResponse>
	{
		private readonly UserManager<FileManagerUser> _userManager = userManager;
		private readonly ITokenService _tokenService = tokenService;
		private readonly IConfiguration _config = config;

		public async Task<UserAuthResponse> Handle(UserAuthRequest request, CancellationToken cancellationToken)
		{
			var managedUser = await _userManager.FindByEmailAsync(request.Email);
			if (managedUser == null)
			{
				return new UserAuthResponse
				{
					FailedMessage = $"No accounts registered with {request.Email}"
				};
			}

			if (!await _userManager.CheckPasswordAsync(managedUser, request.Password))
			{
				return new UserAuthResponse
				{
					FailedMessage = $"Incorrect password for {request.Email}"
				};
			}

			var roles = await _userManager.GetRolesAsync(managedUser);

			var (accessToken, accessTokenExpiryDateTime) = _tokenService.CreateAccessToken(managedUser, roles);
			managedUser.RefreshToken = _tokenService.CreateRefreshToken();
			managedUser.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(_config.GetSection("Jwt:RefreshTokenValidityInDays").Get<int>());

			var updatingResult = await _userManager.UpdateAsync(managedUser);

			if (!updatingResult.Succeeded)
			{
				return new UserAuthResponse
				{
					FailedMessage = $"Cannot update user information for {request.Email}"
				};
			}



			return new UserAuthResponse
			{
				AccessToken = accessToken,
				AccessTokenExpiryUtc = accessTokenExpiryDateTime,
				RefreshToken = managedUser.RefreshToken,
				UserName = managedUser.UserName,
				Roles = roles,
				UserId = managedUser.Id,
				Email = request.Email
			};
		}
	}
}
