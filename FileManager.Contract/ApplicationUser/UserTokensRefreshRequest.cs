using FileManager.Contract.Models;
using MediatR;

namespace FileManager.Contract.ApplicationUser
{
	public class UserTokensRefreshRequest : IRequest<UserTokensRefreshResponse>
	{
		public JwtTokensModel Tokens { get; set; }
	}
}
