using FileManager.Contract.Courses;
using FileManager.Contract.Default;
using FileManager.DAL.Domain.PianoMentor.Courses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FileManager.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class CoursesController(ControllersHelper controllersHelper) : ControllerBase
	{
		private readonly ControllersHelper _controllersHelper = controllersHelper;

		[HttpGet]	
		public async Task<IActionResult> GetCourses([FromQuery] long userId)
		{
			return await _controllersHelper.SendRequet<GetCoursesRequest, GetCoursesResponse>(new GetCoursesRequest { UserId = userId });
		}

		[HttpGet]
		public async Task<IActionResult> GetCourseItems([FromQuery] long userId, [FromQuery] int courseId)
		{
			if (courseId <= 0)
			{
				return BadRequest("Course id cannot be less or equals zero");
			}

			return await _controllersHelper.SendRequet<GetCourseItemsRequest, GetCourseItemsResponse>(new GetCourseItemsRequest { UserId = userId, CourseId = courseId });
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> SetCourseViaAdmin([FromBody] SetNewCoursesViaAdminRequest request)
		{
			if (!_controllersHelper.CheckUserPermissions(User, request.UserId))
			{
				return Unauthorized("Wrong user id");
			}

			return await _controllersHelper.SendRequet<SetNewCoursesViaAdminRequest, DefaultResponse>(request);
		}
	}
}
