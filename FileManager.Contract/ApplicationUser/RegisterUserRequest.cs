using MediatR;

namespace FileManager.Contract.ApplicationUser
{
	public class RegisterUserRequest : IRequest<RegisterUserResponse>
	{
		public string UserName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string PasswordConfirm { get; set; }
		public IEnumerable<string> Roles { get; set; }
	}
}
