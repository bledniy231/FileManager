using MediatR;

namespace FileManager.Contract.ApplicationUser
{
	public class RevokeUsersInRoleRequest(string? role) : IRequest<Unit>
	{
		public string? Role { get; set; } = role;
	}
}
