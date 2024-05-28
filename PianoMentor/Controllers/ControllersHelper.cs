using PianoMentor.Contract.Default;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace PianoMentor.Controllers
{
	public class ControllersHelper(IServiceProvider serviceProvider) : ControllerBase
	{
		public bool IsUserAdmin(ClaimsPrincipal user, out long userId)
		{
			var userRoles = user.FindFirstValue(ClaimTypes.Role)?.Split(' ');
			string? userIdFromClaims = user.FindFirstValue(ClaimTypes.NameIdentifier);
			
			return long.TryParse(userIdFromClaims, out userId) && userRoles?.Contains("Admin") == true;
		}

		public bool CheckUserPermissions(ClaimsPrincipal user, long userId)
		{
			var userIdFromClaims = user.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userIdFromClaims) || !long.TryParse(userIdFromClaims, out long parsedUserId))
			{
				return false;
			}

			var userRoles = user.FindFirstValue(ClaimTypes.Role)?.Split(' ');
			return parsedUserId == userId || userRoles?.Contains("Admin") == true;
		}


		public bool IdentifyUser(ClaimsPrincipal user, long userId)
		{
			string? userIdFromClaims = user.FindFirstValue(ClaimTypes.NameIdentifier);

			return userIdFromClaims != null && long.TryParse(userIdFromClaims, out long parsedUserId) && parsedUserId == userId;
		}

		public async Task<IActionResult> SendRequest<TRequest, TResponse>(TRequest request)
			where TRequest : IRequest<TResponse>
			where TResponse : DefaultResponse
		{
			using var scope = serviceProvider.CreateScope();
			var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
			var response = await mediator.Send(request);
			if (!response.Errors.IsNullOrEmpty())
			{
				return BadRequest(response.Errors);
			}

			return Ok(response);
		}
	}
}
