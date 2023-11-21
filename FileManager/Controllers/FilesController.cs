using FileManager.Attributes;
using FileManager.Contract.Files;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FileManager.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class FilesController(IMediator mediator) : ControllerBase
	{
		private readonly IMediator _mediator = mediator;

		[HttpPost]
		[Authorize]
		[Consumes("multipart/form-data")]
		[DisableRequestSizeLimit]
		[DisableFormValueModelBinding]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> UploadFiles([FromQuery] long userId)
		{
			if (!IdentifyUser(userId))
			{
				return BadRequest("Wrong user id");
			}

			var request = new UploadFilesRequest(userId, Request.ContentType, Request.Body);
			var response = await _mediator.Send(request);

			if (!response.IsSuccess)
			{
				return BadRequest(response.Errors);
			}

			return Ok();
		}

		[HttpGet]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<GetListOfUploadedFilesResponse>> GetListOfUploadedFiles([FromQuery] long userId)
		{
			if (!CheckUserPermissions(userId))
			{
				return BadRequest("Wrong user id");
			}

			var request = new GetListOfUploadedFilesRequest(userId);
			var response = await _mediator.Send(request);

			if (!response.IsSuccess)
			{
				return BadRequest(response.Message);
			}

			return Ok(response);
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
		public async Task<ActionResult> DownloadFiles([FromQuery] long userId, [FromQuery] long dataSetId, [FromQuery] int? dataId)
		{
			if (!CheckUserPermissions(userId))
			{
				return BadRequest("Wrong user id");
			}

			var request = new DownloadFilesRequest(dataSetId, dataId);
			var response = await _mediator.Send(request);

			if (!response.IsSuccess)
			{
				return BadRequest(response.Message);
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
			if (!IdentifyUser(userId))
			{
				return BadRequest("Wrong user id");
			}

			var response = await _mediator.Send(new CheckFilesUploadStatusRequest(userId));
			return response != null ? Ok(response) : NotFound();
		}


		[HttpGet]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<EncryptOneTimeLinkResponse>> CreateOneTimeDownloadLink([FromQuery] long userId, [FromQuery] long dataSetId, [FromQuery] int? dataId)
		{
			if (!CheckUserPermissions(userId))
			{
				return BadRequest("Wrong user id");
			}

			var request = new EncryptOneTimeLinkRequest(dataSetId, dataId);

			try
			{
				var response = await _mediator.Send(request);
				return response;
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


		[HttpGet]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> DownloadFilesViaLink([FromQuery] string token)
		{
			var decryptRequest = new DecryptOneTimeLinkRequest(token);
			try
			{
				var decryptResponse = await _mediator.Send(decryptRequest);

				var downloadRequest = new DownloadFilesRequest(decryptResponse.DataSetId, decryptResponse.DataId);
				var downloadResponse = await _mediator.Send(downloadRequest);

				if (!downloadResponse.IsSuccess)
				{
					return BadRequest(downloadResponse.Message);
				}

				var fsResult = new FileStreamResult(downloadResponse.FileStream, downloadResponse.ContentType)
				{
					FileDownloadName = downloadResponse.FileDownloadName
				};

				await _mediator.Send(new DeleteOneTimeLinkFromDbRequest(token));

				return fsResult;
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		

		private bool CheckUserPermissions(long userId)
		{
			string? userIdFromClaims = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userIdFromClaims != null && long.TryParse(userIdFromClaims, out long parsedUserId))
			{
				var userRoles = User.FindFirstValue(ClaimTypes.Role)?.Split(' ');
				if (parsedUserId != userId && userRoles != null && !userRoles.Contains("Admin"))
				{
					return false;
				}

				return true;
			}
			else
			{
				return false;
			}
		}

		private bool IdentifyUser(long userId)
		{
			string? userIdFromClaims = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userIdFromClaims == null || !long.TryParse(userIdFromClaims, out long parsedUserId) || parsedUserId != userId)
			{
				return false;
			}

			return true;
		}
	}
}
