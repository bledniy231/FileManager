using FileManager.Contract.Default;
using MediatR;

namespace FileManager.Contract.ApplicationUser
{
	public class RevokeUserRequest(string username) : IRequest<DefaultResponse>
	{
		public string Username { get; set; } = username;
	}
}
