using PianoMentor.Attributes;
using PianoMentor.Contract.Default;
using PianoMentor.Contract.Files;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace PianoMentor.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class FilesController(
		ControllersHelper controllersHelper,
		IMediator mediator) : ControllerBase
	{
		private readonly ControllersHelper _controllersHelper = controllersHelper;
		private readonly IMediator _mediator = mediator;
		[HttpPost]
		[Authorize]
		[Consumes("multipart/form-data")]
		[DisableRequestSizeLimit]
		[DisableFormValueModelBinding]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UploadUserFiles([FromQuery] long userId)
		{
			if (!_controllersHelper.IdentifyUser(User, userId))
			{
				return Unauthorized("Wrong user id");
			}

			var request = new UploadFilesRequest(userId, null, Request.ContentType, Request.Body);
			return await _controllersHelper.SendRequet<UploadFilesRequest, DefaultResponse>(request);
		}

		[HttpGet]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetListOfUploadedFiles([FromQuery] long userId)
		{
			if (!_controllersHelper.CheckUserPermissions(User, userId))
			{
				return Unauthorized("Wrong user id");
			}

			var request = new GetListOfUploadedFilesRequest(userId);
			return await _controllersHelper.SendRequet<GetListOfUploadedFilesRequest, GetListOfUploadedFilesResponse>(request);
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
			if (!_controllersHelper.CheckUserPermissions(User, userId))
			{
				return Unauthorized("Wrong user id");
			}

			var request = new DownloadFilesRequest(dataSetId, dataId);
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

		[HttpGet]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<(int FilesCountAlreadyUploaded, float PercentageCurrentFile)?>> CheckFilesUploadStatus([FromQuery] long userId)
		{
			if (!_controllersHelper.IdentifyUser(User, userId))
			{
				return Unauthorized("Wrong user id");
			}

			var response = await _mediator.Send(new CheckFilesUploadStatusRequest(userId));
			return response != null ? Ok(response) : NotFound();
		}


		[HttpGet]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateOneTimeDownloadLink([FromQuery] long userId, [FromQuery] long dataSetId, [FromQuery] int? dataId)
		{
			if (!_controllersHelper.CheckUserPermissions(User, userId))
			{
				return Unauthorized("Wrong user id");
			}

			var request = new EncryptOneTimeLinkRequest(dataSetId, dataId);

			try
			{
				return await _controllersHelper.SendRequet<EncryptOneTimeLinkRequest, EncryptOneTimeLinkResponse>(request);
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
			var decryptResponse = await _mediator.Send(new DecryptOneTimeLinkRequest(token));

			if (decryptResponse.Errors != null)
			{
				return BadRequest(decryptResponse.Errors);
			}

			var downloadResponse = await _mediator.Send(new DownloadFilesRequest(decryptResponse.DataSetId, decryptResponse.DataId));

			if (!downloadResponse.Errors.IsNullOrEmpty())
			{
				return BadRequest(downloadResponse.Errors);
			}

			var fsResult = new FileStreamResult(downloadResponse.FileStream, downloadResponse.ContentType)
			{
				FileDownloadName = downloadResponse.FileDownloadName
			};

			await _mediator.Send(new DeleteOneTimeLinkFromDbRequest(token));

			return fsResult;
		}
	}
}
