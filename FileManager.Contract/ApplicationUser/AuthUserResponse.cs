using FileManager.Contract.Models.JwtTokens;

namespace FileManager.Contract.ApplicationUser
{
	public class AuthUserResponse
	{
		public JwtTokensModel JwtTokensModel { get; set; }
		public string Email { get; set; }
		public string UserName { get; set; }
		public IEnumerable<string> Roles { get; set; }
		public long UserId { get; set; }


		public string? FailedMessage { get; set; } = null;
	}
}
