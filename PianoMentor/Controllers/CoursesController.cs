using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PianoMentor.Contract.Courses;
using PianoMentor.Contract.Default;
using PianoMentor.Contract.Exercises;
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
		[HttpGet]	
		public async Task<IActionResult> GetCourses([FromQuery] long userId)
		{
			return await controllersHelper.SendRequest<GetCoursesRequest, GetCoursesResponse>(new GetCoursesRequest { UserId = userId });
		}

		[HttpGet]
		public async Task<IActionResult> GetCourseItems([FromQuery] long userId, [FromQuery] int courseId)
		{
			if (courseId <= 0)
			{
				return BadRequest("Course id cannot be less or equals zero");
			}

			return await controllersHelper.SendRequest<GetCourseItemsRequest, GetCourseItemsResponse>(new GetCourseItemsRequest { UserId = userId, CourseId = courseId });
		}

		[HttpGet]
		public async Task<IActionResult> GetCourseItemsWithFilter([FromQuery] long userId, [FromQuery] string filter)
		{
			var courseItemTypeId = Enum.TryParse(typeof(CourseItemTypesEnum), filter, out var result) ? (int)result : (int)CourseItemTypesEnum.Lecture;
			return await controllersHelper.SendRequest<GetCourseItemsWithFilterRequest, GetCourseItemsResponse>(new GetCourseItemsWithFilterRequest(userId, courseItemTypeId));
		}

		[HttpPut]
		[Authorize]
		public async Task<IActionResult> SetCourseItemProgress([FromBody] SetCourseItemProgressRequest request)
		{
			return await controllersHelper.SendRequest<SetCourseItemProgressRequest, DefaultResponse>(request);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> SetCourseViaAdmin([FromBody] SetNewCoursesViaAdminRequest request)
		{
			if (!controllersHelper.IsUserAdmin(User, out long _))
			{
				return Unauthorized("You aren't administrator");
			}

			return await controllersHelper.SendRequest<SetNewCoursesViaAdminRequest, DefaultResponse>(request);
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

			return await controllersHelper.SendRequest<GetQuizRequest, GetQuizResponse>(request);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> SetQuizUserAnswers([FromBody] SetQuizUserAnswersRequest request)
		{
			return await controllersHelper.SendRequest<SetQuizUserAnswersRequest, SetQuizUserAnswersResponse>(request);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> SetNewQuiz([FromBody] SetNewQuizRequest request)
		{
			if (!controllersHelper.IsUserAdmin(User, out long _))
			{
				return Unauthorized("You aren't administrator");
			}

			return await controllersHelper.SendRequest<SetNewQuizRequest, DefaultResponse>(request);
		}
		
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetExerciseTask([FromQuery] int courseItemId)
		{
			if (courseItemId <= 0)
			{
				return BadRequest("Course item id cannot be less or equals zero");
			}

			return await controllersHelper.SendRequest<GetExerciseTaskRequest, GetExerciseTaskResponse>(new GetExerciseTaskRequest(courseItemId));
		}
		
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> SetNewExerciseTask([FromBody] SetExercisesTasksRequest request)
		{
			if (!controllersHelper.IsUserAdmin(User, out long _))
			{
				return Unauthorized("You aren't administrator");
			}

			return await controllersHelper.SendRequest<SetExercisesTasksRequest, DefaultResponse>(request);
		}
	}
}
