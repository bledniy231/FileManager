using FileManager.Contract.Courses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FileManager.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class CoursesController(IMediator mediator) : ControllerBase
	{
		private readonly IMediator _mediator = mediator;

		[HttpGet]	
		public async Task<IActionResult> GetCourses([FromQuery] GetCoursesRequest? request = null)
		{
			if (request != null && !IdentifyUser(request.UserId))
			{
				return Unauthorized();
			}

			return Ok(await _mediator.Send(request ?? new GetCoursesRequest()));
		}

		[HttpGet]
		public async Task<IActionResult> GetCourseItems([FromQuery] GetCourseItemsRequest request)
		{
			if (!IdentifyUser(request.UserId))
			{
				return Unauthorized();
			}

			return Ok(await _mediator.Send(request));
		}

		private bool IdentifyUser(long userId)
		{
			string? userIdFromClaims = User.FindFirstValue(ClaimTypes.NameIdentifier);

			return userIdFromClaims != null && long.TryParse(userIdFromClaims, out long parsedUserId) && parsedUserId == userId;
		}
	}
}
