using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FileManager.BLL.TokenService
{
	public class TokenService(
		IConfiguration config,
		IDistributedCache cache,
		IHttpContextAccessor httpContextAccessor) : ITokenService
	{
		private readonly IConfiguration _config = config;
		private readonly IDistributedCache _cache = cache;
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

		public (string token, DateTime accessTokenExpiry) CreateAccessToken(DAL.Domain.Identity.FileManagerUser user, IEnumerable<string> userRoles)
		{
			var (token, accessTokenExpiry) = CreateJwtToken(user, userRoles);
			var tokenHandler = new JwtSecurityTokenHandler();
			return (tokenHandler.WriteToken(token), accessTokenExpiry);
		}

		private (JwtSecurityToken token, DateTime accessTokenExpiry) CreateJwtToken(DAL.Domain.Identity.FileManagerUser user, IEnumerable<string> userRoles)
		{
			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
			int expirationTimeInMin = _config.GetSection("Jwt:Expire").Get<int>();
			var expiryDateTime = DateTime.UtcNow.AddMinutes(expirationTimeInMin);
			var claims = new List<Claim> 
			{
				new (ClaimTypes.Email, user.Email),
				new (ClaimTypes.Name, user.UserName),
				new (ClaimTypes.NameIdentifier, user.Id.ToString()),
				new (ClaimTypes.Role, userRoles.Aggregate((ur1, ur2) => ur1 + ' ' + ur2))
			};

			return (new JwtSecurityToken(
				_config["Jwt:Issuer"],
				_config["Jwt:Audience"],
				claims: claims,
				expires: expiryDateTime,
				signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)),
				expiryDateTime);
		}

		public string CreateRefreshToken()
		{
			byte[] randomNumber = new byte[64];

			using var rng = RandomNumberGenerator.Create();
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

		public bool IsCurrentUserActiveToken()
			=> IsActiveUserToken(GetCurrentUserToken());

		public void DeactivateCurrentUserToken()
			=> DeactivateUserToken(GetCurrentUserToken());

		public bool IsActiveUserToken(string token)
			=> _cache.GetString(GetCacheKey(token)) == null;

		public void DeactivateUserToken(string token)
			=> _cache.SetString(GetCacheKey(token),
				" ", new DistributedCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_config.GetSection("Jwt:Expire").Get<int>())
				});

		private string GetCurrentUserToken()
		{
			var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers.Authorization;

			if (!authorizationHeader.HasValue)
			{
				return string.Empty;
			}

			return authorizationHeader.Value == StringValues.Empty
				? string.Empty
				: authorizationHeader.Value.Single().Split(" ").Last();
		}

		private static string GetCacheKey(string token)
			=> $"tokens:{token}:deactivated";
	}
}
