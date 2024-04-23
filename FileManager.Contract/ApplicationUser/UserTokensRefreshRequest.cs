using FileManager.Contract.Models.JwtTokens;
using MediatR;

namespace FileManager.Contract.ApplicationUser
{
    public class UserTokensRefreshRequest : IRequest<UserTokensRefreshResponse>
	{
		public JwtTokensModel Tokens { get; set; }
	}
}
