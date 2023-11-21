using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FileManager.BLL.TokenService
{
	public class TokenService(IConfiguration config) : ITokenService
	{
		private readonly IConfiguration _config = config;

		public string CreateAccessToken(DAL.Domain.Identity.FileManagerUser user, IEnumerable<string> userRoles)
		{
			var token = CreateJwtToken(user, userRoles);
			var tokenHandler = new JwtSecurityTokenHandler();
			return tokenHandler.WriteToken(token);
		}

		private JwtSecurityToken CreateJwtToken(DAL.Domain.Identity.FileManagerUser user, IEnumerable<string> userRoles)
		{
			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
			int expirationTimeInMin = _config.GetSection("Jwt:Expire").Get<int>();
			var claims = new List<Claim> 
			{
				new (ClaimTypes.Email, user.Email),
				new (ClaimTypes.Name, user.UserName),
				new (ClaimTypes.NameIdentifier, user.Id.ToString()),
				new (ClaimTypes.Role, userRoles.Aggregate((ur1, ur2) => ur1 + ' ' + ur2))
			};

			return new JwtSecurityToken(
				_config["Jwt:Issuer"],
				_config["Jwt:Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(expirationTimeInMin),
				signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
		}

		public string CreateRefreshToken()
		{
			byte[] randomNumber = new byte[64];

			using RandomNumberGenerator rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);

			return Convert.ToBase64String(randomNumber);
		}
		
		public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? accessToken)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = false,
				ValidateIssuer = false,
				ValidateIssuerSigningKey = true,
				ValidateLifetime = false,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!))
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);

			if (securityToken is not JwtSecurityToken jwtSecurityToken
				|| !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
			{
				return null;
			}

			return principal;
		}
	}
}
