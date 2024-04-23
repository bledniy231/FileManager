using FileManager.BLL.TokenService;
using FileManager.Contract.ApplicationUser;
using FileManager.DAL.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FileManager.BLL.ApplicationUser
{
	internal class UserTokensRefreshHandler(
		UserManager<FileManagerUser> userManager,
		ITokenService tokenService) : IRequestHandler<UserTokensRefreshRequest, UserTokensRefreshResponse>
	{
		private readonly UserManager<FileManagerUser> _userManager = userManager;
		private readonly ITokenService _tokenService = tokenService;

		public async Task<UserTokensRefreshResponse> Handle(UserTokensRefreshRequest request, CancellationToken cancellationToken)
		{
			var principalFromExpToken = _tokenService.GetPrincipalFromExpiredToken(request.Tokens.AccessToken);

			if (principalFromExpToken == null)
			{
				return new UserTokensRefreshResponse
				{
					IsSuccess = false,
					Message = "Invalid token"
				};
			}

			string? userId = principalFromExpToken.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _userManager.FindByIdAsync(userId);

			if (user == null)
			{
				return new UserTokensRefreshResponse
				{
					IsSuccess = false,
					Message = "Connot find user, who owns this access token"
				};
			}

			if (!user.RefreshToken.Equals(request.Tokens.RefreshToken) || user.RefreshTokenExpireTime < DateTime.UtcNow)
			{
				return new UserTokensRefreshResponse
				{
					IsSuccess = false,
					Message = $"User's \"{user.UserName}\" refresh token does not match with sended refresh token or refresh token has been already expired"
				};
			}

			var userRoles = await _userManager.GetRolesAsync(user);
			var (newAccessToken, newAccessTokenExpiryDateTime) = _tokenService.CreateAccessToken(user, userRoles);
			string newRefreshToken = _tokenService.CreateRefreshToken();
			user.RefreshToken = newRefreshToken;

			var updatingResult = await _userManager.UpdateAsync(user);

			if (!updatingResult.Succeeded)
			{
				return new UserTokensRefreshResponse
				{
					IsSuccess = false,
					Message = $"Cannot update user's \"{user.UserName}\" tokens in database"
				};
			}

			return new UserTokensRefreshResponse
			{
				NewTokens = new Contract.Models.JwtTokensModel
				{
					AccessToken = newAccessToken,
					RefreshToken = newRefreshToken
				},
				IsSuccess = true
			};
		}
	}
}
