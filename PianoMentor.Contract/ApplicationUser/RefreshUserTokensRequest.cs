using MediatR;
using PianoMentor.Contract.Models.JwtTokens;

namespace PianoMentor.Contract.ApplicationUser
{
	public class RefreshUserTokensRequest : IRequest<RefreshUserTokensResponse>
	{
		public JwtTokensModel Tokens { get; set; }
	}
}
