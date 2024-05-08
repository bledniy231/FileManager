using PianoMentor.BLL.TokenService;
using PianoMentor.Contract.ApplicationUser;
using PianoMentor.DAL.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace PianoMentor.BLL.ApplicationUser
{
	internal class RefreshUserTokensHandler(
		UserManager<PianoMentorUser> userManager,
		ITokenService tokenService) : IRequestHandler<RefreshUserTokensRequest, RefreshUserTokensResponse>
	{
		private readonly UserManager<PianoMentorUser> _userManager = userManager;
		private readonly ITokenService _tokenService = tokenService;

		public async Task<RefreshUserTokensResponse> Handle(RefreshUserTokensRequest request, CancellationToken cancellationToken)
		{
			var principalFromExpToken = _tokenService.GetPrincipalFromExpiredToken(request.Tokens.AccessToken);

			if (principalFromExpToken == null)
			{
				return new RefreshUserTokensResponse(["Invalid token"]);
			}

			string? userId = principalFromExpToken.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
			{
				return new RefreshUserTokensResponse(["Cannot find user id in access token"]);
			}

			var user = await _userManager.FindByIdAsync(userId);
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

			var userRoles = await _userManager.GetRolesAsync(user);
			var (newAccessToken, newAccessTokenExpiryDateTime) = _tokenService.CreateAccessToken(user, userRoles);
			string newRefreshToken = _tokenService.CreateRefreshToken();
			user.RefreshToken = newRefreshToken;

			var updatingResult = await _userManager.UpdateAsync(user);

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
