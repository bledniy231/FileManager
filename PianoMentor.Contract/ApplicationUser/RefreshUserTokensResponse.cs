using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.JwtTokens;

namespace PianoMentor.Contract.ApplicationUser
{
    public class RefreshUserTokensResponse(string[]? errors) : DefaultResponse(errors)
	{
		public JwtTokensModel NewTokens { get; set; }
	}
}
