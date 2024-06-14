using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PianoMentor.BLL.Services.TokenService;
using PianoMentor.Contract.ApplicationUser;
using PianoMentor.DAL.Models.Identity;

namespace PianoMentor.BLL.UseCases.ApplicationUser
{
	internal class RefreshUserTokensHandler(
		UserManager<PianoMentorUser> userManager,
		ITokenService tokenService) : IRequestHandler<RefreshUserTokensRequest, RefreshUserTokensResponse>
	{
		public async Task<RefreshUserTokensResponse> Handle(RefreshUserTokensRequest request, CancellationToken cancellationToken)
		{
			var principalFromExpToken = tokenService.GetPrincipalFromExpiredToken(request.Tokens.AccessToken);

			if (principalFromExpToken == null)
			{
				return new RefreshUserTokensResponse(["Invalid token"]);
			}

			string? userId = principalFromExpToken.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
			{
				return new RefreshUserTokensResponse(["Cannot find user id in access token"]);
			}

			var user = await userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return new RefreshUserTokensResponse(["Connot find user, who owns this access token"]);
			}
			if (user.RefreshToken == null)
			{
				return new RefreshUserTokensResponse([$"User's \"{user.UserName}\" refresh token is null"]);
			}
			if (!user.RefreshToken.Equals(request.Tokens.RefreshToken) || user.RefreshTokenExpireTime < DateTime.UtcNow)
			{
				return new RefreshUserTokensResponse([$"User's \"{user.UserName}\" refresh token does not match with sended refresh token or refresh token has been already expired"]);
			}

			var userRoles = await userManager.GetRolesAsync(user);
			var (newAccessToken, newAccessTokenExpiryDateTime) = tokenService.CreateAccessToken(user, userRoles);
			string newRefreshToken = tokenService.CreateRefreshToken();
			user.RefreshToken = newRefreshToken;

			var updatingResult = await userManager.UpdateAsync(user);

			if (!updatingResult.Succeeded)
			{
				return new RefreshUserTokensResponse([$"Cannot update user's \"{user.UserName}\" tokens in database"]);
			}

			return new RefreshUserTokensResponse(null)
			{
				NewTokens = new Contract.Models.JwtTokens.JwtTokensModel
				{
					AccessToken = newAccessToken,
					AccessTokenExpireTime = newAccessTokenExpiryDateTime,
					RefreshToken = newRefreshToken,
					RefreshTokenExpireTime = request.Tokens.RefreshTokenExpireTime
				}
			};
		}
	}
}
