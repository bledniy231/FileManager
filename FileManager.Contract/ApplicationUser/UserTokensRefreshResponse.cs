using FileManager.Contract.Models.JwtTokens;

namespace FileManager.Contract.ApplicationUser
{
    public class UserTokensRefreshResponse
	{
		public JwtTokensModel NewTokens { get; set; }
		public bool IsSuccess { get; set; }
		public string Message { get; set; }
	}
}
