using PianoMentor.Contract.Models.JwtTokens;
using MediatR;

namespace PianoMentor.Contract.ApplicationUser
{
    public class RefreshUserTokensRequest : IRequest<RefreshUserTokensResponse>
	{
		public JwtTokensModel Tokens { get; set; }
	}
}
