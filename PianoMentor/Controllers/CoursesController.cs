using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PianoMentor.Attributes;
using PianoMentor.Contract.Courses;
using PianoMentor.Contract.Default;
using PianoMentor.Contract.Files;
using PianoMentor.Contract.Models.PianoMentor.Courses;
using PianoMentor.Contract.Quizzes;
using PianoMentor.Contract.Statistics;

namespace PianoMentor.Controllers
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

		[HttpPut]
		[Authorize]
		public async Task<IActionResult> SetCourseItemProgress([FromBody] SetCourseItemProgressRequest request)
		{
			return await _controllersHelper.SendRequet<SetCourseItemProgressRequest, DefaultResponse>(request);
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
		public async Task<IActionResult> DownloadCourseItemFile([FromQuery] int courseItemId)
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

			var downloadRequest = new DownloadCourseItemFileRequest(courseItemId);
			var response = await _mediator.Send(downloadRequest);

			if (response.Errors != null && response.Errors.Length > 0)
			{
				return BadRequest(response.Errors);
			}

			return new FileStreamResult(response.FileStream, response.ContentType)
			{
				FileDownloadName = response.FileDownloadName
			};
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetQuiz([FromQuery] int courseId, [FromQuery] int courseItemId, [FromQuery] long userId)
		{
			if (courseId <= 0 || courseItemId <= 0 || userId <= 0)
			{
				return BadRequest("Course id, course item id or user id cannot be less or equals zero");
			}

			var request = new GetQuizRequest()
			{
				CourseId = courseId,
				CourseItemId = courseItemId,
				UserId = userId
			};

			return await _controllersHelper.SendRequet<GetQuizRequest, GetQuizResponse>(request);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> SetQuizUserAnswers([FromBody] SetQuizUserAnswersRequest request)
		{
			return await _controllersHelper.SendRequet<SetQuizUserAnswersRequest, DefaultResponse>(request);
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> DownloadQuizQuestionFile([FromQuery] long dataSetId)
		{
			var request = new DownloadFilesRequest(dataSetId, null, false);
			var response = await _mediator.Send(request);

			if (!response.Errors.IsNullOrEmpty())
			{
				return BadRequest(response.Errors);
			}

			return new FileStreamResult(response.FileStream, response.ContentType)
			{
				FileDownloadName = response.FileDownloadName
			};
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> SetNewQuiz([FromBody] SetNewQuizRequest request)
		{
			if (!_controllersHelper.IsUserAdmin(User, out long _))
			{
				return Unauthorized("You aren't administrator");
			}

			return await _controllersHelper.SendRequet<SetNewQuizRequest, DefaultResponse>(request);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> UploadQuestionImage([FromBody] UploadQuestionImageRequest request)
		{
			if (!_controllersHelper.IsUserAdmin(User, out long _))
			{
				return Unauthorized("You aren't administrator");
			}

			return await _controllersHelper.SendRequet<UploadQuestionImageRequest, DefaultResponse>(request);
		}
	}
}
