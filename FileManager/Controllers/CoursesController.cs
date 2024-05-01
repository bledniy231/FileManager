using FileManager.Attributes;
using FileManager.Contract.Courses;
using FileManager.Contract.Default;
using FileManager.Contract.Files;
using FileManager.Contract.Models.PianoMentor.Courses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileManager.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class CoursesController(
		ControllersHelper controllersHelper,
		IMediator mediator) : ControllerBase
	{
		private readonly ControllersHelper _controllersHelper = controllersHelper;
		private readonly IMediator _mediator = mediator;

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
			if (!_controllersHelper.IsUserAdmin(User, out long _))
			{
				return Unauthorized("You aren't administrator");
			}

			return await _controllersHelper.SendRequet<SetNewCoursesViaAdminRequest, DefaultResponse>(request);
		}

		[HttpPost]
		[Authorize]
		[Consumes("multipart/form-data")]
		[DisableRequestSizeLimit]
		[DisableFormValueModelBinding]
		public async Task<IActionResult> UploadLecturePdf([FromQuery] int courseItemId)
		{
			if (!_controllersHelper.IsUserAdmin(User, out long userId))
			{
				return Unauthorized("You aren't administrator");
			}

			var isCourseItemExistResponse = await _mediator.Send(new CheckIfCourseItemExistsRequest
			{
				CourseItemId = courseItemId,
				CourseItemTypeId = (int)CourseItemTypesEnumeration.Lecture
			});

			if (isCourseItemExistResponse.Errors != null && isCourseItemExistResponse.Errors.Length > 0)
			{
				return NotFound($"Course item not found, errors: {string.Join("; ", isCourseItemExistResponse.Errors)}");
			}

			var uploadRequest = new UploadFilesRequest(userId, courseItemId, Request.ContentType, Request.Body);
			return await _controllersHelper.SendRequet<UploadFilesRequest, DefaultResponse>(uploadRequest);
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DownloadLecturePdf([FromQuery] int courseItemId)
		{
			var isCourseItemExistResponse = await _mediator.Send(new CheckIfCourseItemExistsRequest
			{
				CourseItemId = courseItemId,
				CourseItemTypeId = (int)CourseItemTypesEnumeration.Lecture
			});

			if (isCourseItemExistResponse.Errors != null && isCourseItemExistResponse.Errors.Length > 0)
			{
				return NotFound($"Course item not found, errors: {string.Join("; ", isCourseItemExistResponse.Errors)}");
			}

			var downloadRequest = new DownloadLecturePdfRequest(courseItemId);
			return await _controllersHelper.SendRequet<DownloadLecturePdfRequest, DownloadFilesResponse>(downloadRequest);
		}
	}
}
