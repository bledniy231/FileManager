using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PianoMentor.Certificates;
using PianoMentor.Contract.ApplicationUser;
using PianoMentor.Contract.Models.PianoMentor.ApplicationUser;
using PianoMentor.Contract.Statistics;

namespace PianoMentor.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class ApplicationUserController(
		IMediator mediator,
		ControllersHelper controllersHelper) : ControllerBase
	{
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<AuthUserResponse>> Login([FromBody] AuthUserRequest request)
		{
			var userAuthResponse = await mediator.Send(request);

			if (userAuthResponse.FailedMessage != null)
			{
				return BadRequest(userAuthResponse.FailedMessage);
			}

			return Ok(userAuthResponse);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<AuthUserResponse>> Register([FromBody] RegisterUserRequest request)
		{
			var userRegisterResponse = await mediator.Send(request);
			
			if (!userRegisterResponse.Errors.IsNullOrEmpty())
			{
				return BadRequest(userRegisterResponse.Errors);
			}
			
			return await Login(new AuthUserRequest(userRegisterResponse.Email, userRegisterResponse.Password));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<RefreshUserTokensResponse>> RefreshUserTokens([FromBody] RefreshUserTokensRequest request)
		{
			var userRefreshResponse = await mediator.Send(request);

			if (!userRefreshResponse.Errors.IsNullOrEmpty())
			{
				return BadRequest(userRefreshResponse.Errors);
			}

			return Ok(userRefreshResponse);
		}

		[Authorize]
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<string[]>> RevokeUser([FromQuery] string username)
		{
			var revokeUserResponse = await mediator.Send(new RevokeUserRequest(username));

			if (!revokeUserResponse.Errors.IsNullOrEmpty())
			{
				return BadRequest(revokeUserResponse.Errors);
			}

			return Ok();
		}

		[Authorize(Policy = "RequireAdminRole")]
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> RevokeUsersInRole([FromQuery] string role)
			=> Ok(await mediator.Send(new RevokeUsersInRoleRequest(role)));

		[Authorize]
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Logout()
		{
			//await mediator.Send(new LogoutUserRequest());

			return NoContent();
		}
		
		[Authorize]
		[HttpPost]
		public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
	        if (!controllersHelper.IdentifyUser(User, model.UserId))
	        {
		        return Unauthorized("Wrong user id");
	        }

	        var request = new ChangePasswordRequest
	        {
		        User = User,
		        OldPassword = model.OldPassword,
		        NewPassword = model.NewPassword,
		        RepeatNewPassword = model.RepeatNewPassword
	        };
	        
            var userUpdateResponse = await mediator.Send(request);

            if (!userUpdateResponse.Errors.IsNullOrEmpty())
            {
                return BadRequest(userUpdateResponse.Errors);
            }

            return Ok(userUpdateResponse);
        }

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetUserStatistics([FromQuery] long userId)
		{
			if (!controllersHelper.IdentifyUser(User, userId))
			{
				return Unauthorized("Wrong user id");
			}
			
			var coursesUserStatistics = await mediator.Send(new GetUserStatisticsRequest(userId));

			return Ok(coursesUserStatistics);
		}
	}
}
