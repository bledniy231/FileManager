using FileManager.Contract.Default;
using FileManager.Contract.Models.JwtTokens;

namespace FileManager.Contract.ApplicationUser
{
    public class RefreshUserTokensResponse(string[]? errors) : DefaultResponse(errors)
	{
		public JwtTokensModel NewTokens { get; set; }
	}
}
