using System.Security.Claims;
using PianoMentor.DAL.Models.Identity;

namespace PianoMentor.BLL.Services.TokenService
{
	public interface ITokenService
	{
		public (string token, DateTime accessTokenExpiry) CreateAccessToken(PianoMentorUser user, IEnumerable<string> userRoles);
		public string CreateRefreshToken();
		public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? accessToken);

		bool IsCurrentUserActiveToken();
		void DeactivateCurrentUserToken();
		bool IsActiveUserToken(string token);
		void DeactivateUserToken(string token);
	}
}
