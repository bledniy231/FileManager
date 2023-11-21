using MediatR;

namespace FileManager.Contract.ApplicationUser
{
	public class UserAuthRequest(string email, string password) : IRequest<UserAuthResponse>
	{
		public string Email { get; set; } = email;
		public string Password { get; set; } = password;
	}
}
