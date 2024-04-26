using FileManager.Contract.Models.JwtTokens;
using MediatR;

namespace FileManager.Contract.ApplicationUser
{
    public class RefreshUserTokensRequest : IRequest<RefreshUserTokensResponse>
	{
		public JwtTokensModel Tokens { get; set; }
	}
}
