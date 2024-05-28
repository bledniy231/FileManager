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

namespace PianoMentor.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class FilesController(
		ControllersHelper controllersHelper,
		IMediator mediator) : ControllerBase
	{
		[HttpPost]
		[Authorize]
		[Consumes("multipart/form-data")]
		[DisableRequestSizeLimit]
		[DisableFormValueModelBinding]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UploadUserFiles([FromQuery] long userId)
		{
			if (!controllersHelper.IdentifyUser(User, userId))
			{
				return Unauthorized("Wrong user id");
			}

			var request = new UploadFilesRequest(userId, null, Request.ContentType, Request.Body);
			return await controllersHelper.SendRequest<UploadFilesRequest, DefaultResponse>(request);
		}

		[HttpGet]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetListOfUploadedFiles([FromQuery] long userId)
		{
			if (!controllersHelper.CheckUserPermissions(User, userId))
			{
				return Unauthorized("Wrong user id");
			}

			var request = new GetListOfUploadedFilesRequest(userId);
			return await controllersHelper.SendRequest<GetListOfUploadedFilesRequest, GetListOfUploadedFilesResponse>(request);
		}

		/// <summary>
		/// Endpoint для скачивания файлов. Если dataId == null, то будет отправлен весь data set
		/// </summary>
		/// <param name="userId">Идентификатор пользователя</param>
		/// <param name="dataSetId">Идентификатор data set</param>
		/// <param name="dataId">Идентификатор файла из data set. В случае скачивания всего data set, передавать null</param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> DownloadFiles([FromQuery] long userId, [FromQuery] long dataSetId, [FromQuery] int? dataId)
		{
			if (!controllersHelper.CheckUserPermissions(User, userId))
			{
				return Unauthorized("Wrong user id");
			}

			var request = new DownloadFilesRequest(dataSetId, dataId);
			var response = await mediator.Send(request);

			if (!response.Errors.IsNullOrEmpty())
			{
				return BadRequest(response.Errors);
			}

			return new FileStreamResult(response.FileStream, response.ContentType)
			{
				FileDownloadName = response.FileDownloadName
			};
		}

		[HttpGet]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<(int FilesCountAlreadyUploaded, float PercentageCurrentFile)?>> CheckFilesUploadStatus([FromQuery] long userId)
		{
			if (!controllersHelper.IdentifyUser(User, userId))
			{
				return Unauthorized("Wrong user id");
			}

			var response = await mediator.Send(new CheckFilesUploadStatusRequest(userId));
			return response != null ? Ok(response) : NotFound();
		}


		[HttpGet]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateOneTimeDownloadLink([FromQuery] long userId, [FromQuery] long dataSetId, [FromQuery] int? dataId)
		{
			if (!controllersHelper.CheckUserPermissions(User, userId))
			{
				return Unauthorized("Wrong user id");
			}

			var request = new EncryptOneTimeLinkRequest(dataSetId, dataId);

			try
			{
				return await controllersHelper.SendRequest<EncryptOneTimeLinkRequest, EncryptOneTimeLinkResponse>(request);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


		[HttpGet]
		//[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> DownloadFilesViaLink([FromQuery] string token)
		{
			var decryptResponse = await mediator.Send(new DecryptOneTimeLinkRequest(token));

			if (decryptResponse.Errors != null)
			{
				return BadRequest(decryptResponse.Errors);
			}

			var downloadResponse = await mediator.Send(new DownloadFilesRequest(decryptResponse.DataSetId, decryptResponse.DataId));

			if (!downloadResponse.Errors.IsNullOrEmpty())
			{
				return BadRequest(downloadResponse.Errors);
			}

			var fsResult = new FileStreamResult(downloadResponse.FileStream, downloadResponse.ContentType)
			{
				FileDownloadName = downloadResponse.FileDownloadName
			};

			await mediator.Send(new DeleteOneTimeLinkFromDbRequest(token));

			return fsResult;
		}
		
		
				[HttpPost]
		[Authorize]
		[Consumes("multipart/form-data")]
		[DisableRequestSizeLimit]
		[DisableFormValueModelBinding]
		public async Task<IActionResult> UploadLecturePdf([FromQuery] int courseItemId)
		{
			if (!controllersHelper.IsUserAdmin(User, out long userId))
			{
				return Unauthorized("You aren't administrator");
			}

			var isCourseItemExistResponse = await mediator.Send(new CheckIfCourseItemExistsRequest
			{
				CourseItemId = courseItemId,
				CourseItemTypeId = (int)CourseItemTypesEnum.Lecture
			});

			if (isCourseItemExistResponse.Errors != null && isCourseItemExistResponse.Errors.Length > 0)
			{
				return NotFound($"Course item not found, errors: {string.Join("; ", isCourseItemExistResponse.Errors)}");
			}

			var uploadRequest = new UploadFilesRequest(userId, courseItemId, Request.ContentType, Request.Body);
			return await controllersHelper.SendRequest<UploadFilesRequest, DefaultResponse>(uploadRequest);
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DownloadCourseItemFile([FromQuery] int courseItemId)
		{
			var isCourseItemExistResponse = await mediator.Send(new CheckIfCourseItemExistsRequest
			{
				CourseItemId = courseItemId,
				CourseItemTypeId = (int)CourseItemTypesEnum.Lecture
			});

			if (isCourseItemExistResponse.Errors is { Length: > 0 })
			{
				return NotFound($"Course item not found, errors: {string.Join("; ", isCourseItemExistResponse.Errors)}");
			}

			var downloadRequest = new DownloadCourseItemFileRequest(courseItemId);
			var response = await mediator.Send(downloadRequest);

			if (response.Errors is { Length: > 0 })
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
		public async Task<IActionResult> DownloadQuizQuestionFile([FromQuery] long dataSetId)
		{
			var request = new DownloadFilesRequest(dataSetId, 0, false);
			var response = await mediator.Send(request);

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
		[HttpPut]
		[Consumes("multipart/form-data")]
		[DisableRequestSizeLimit]
		[DisableFormValueModelBinding]
		public async Task<IActionResult> UploadQuestionImage([FromQuery] long userId, int questionId)
		{
			if (!controllersHelper.IsUserAdmin(User, out long _))
			{
				return Unauthorized("You aren't administrator");
			}

			var request = new UploadQuestionImageRequest(userId, questionId, Request.ContentType, Request.Body);
			return await controllersHelper.SendRequest<UploadQuestionImageRequest, DefaultResponse>(request);
		}
		
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetUserProfilePhoto([FromQuery] long userId)
		{
			if (!controllersHelper.IdentifyUser(User, userId))
			{
				return Unauthorized("Wrong user id");
			}
			
			var response = await mediator.Send(new GetUserProfilePhotoRequest(userId));
			
			if (response.Errors is { Length: > 0 })
			{
				return BadRequest(response.Errors);
			}

			return new FileStreamResult(response.FileStream, response.ContentType)
			{
				FileDownloadName = response.FileDownloadName
			};
		}
		
		[Authorize]
		[HttpPut]
		[Consumes("multipart/form-data")]
		[DisableRequestSizeLimit]
		[DisableFormValueModelBinding]
		public async Task<IActionResult> UpdateUserProfilePhoto([FromQuery] long userId)
		{
			if (!controllersHelper.IdentifyUser(User, userId))
			{
				return Unauthorized("Wrong user id");
			}
			
			var request = new UploadUserProfilePhotoRequest(userId, Request.ContentType, Request.Body);
			return await controllersHelper.SendRequest<UploadUserProfilePhotoRequest, DefaultResponse>(request);
		}
	}
}
