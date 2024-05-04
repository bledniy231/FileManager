using PianoMentor.Contract.Default;
using MediatR;

namespace PianoMentor.Contract.ApplicationUser
{
	public class RevokeUserRequest(string username) : IRequest<DefaultResponse>
	{
		public string Username { get; set; } = username;
	}
}
