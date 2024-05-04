using MediatR;

namespace PianoMentor.Contract.ApplicationUser
{
	public class AuthUserRequest(string email, string password) : IRequest<AuthUserResponse>
	{
		public string Email { get; set; } = email;
		public string Password { get; set; } = password;
	}
}
