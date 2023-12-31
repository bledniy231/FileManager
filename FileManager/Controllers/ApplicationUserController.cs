﻿using FileManager.Contract.ApplicationUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileManager.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class ApplicationUserController(IMediator mediator) : ControllerBase
	{
		private readonly IMediator _mediator = mediator;

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<UserAuthResponse>> Login([FromBody] UserAuthRequest request)
		{
			var userAuthResponse = await _mediator.Send(request);

			if (!userAuthResponse.IsSuccess)
			{
				return BadRequest(userAuthResponse.Message);
			}

			return Ok(userAuthResponse);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<UserAuthResponse>> Register([FromBody] UserRegisterRequest request)
		{
			var userRegisterResponse = await _mediator.Send(request);
			
			if (!userRegisterResponse.IsSuccess)
			{
				return BadRequest(userRegisterResponse.Errors);
			}
			
			return await Login(new UserAuthRequest(userRegisterResponse.Email, userRegisterResponse.Password));
		}

		[Authorize]
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<UserTokensRefreshResponse>> RefreshUserTokens([FromBody] UserTokensRefreshRequest request)
		{
			var userRefreshResponse = await _mediator.Send(request);

			if (!userRefreshResponse.IsSuccess)
			{
				return BadRequest(userRefreshResponse.Message);
			}

			return Ok(userRefreshResponse);
		}

		[Authorize]
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<string[]>> RevokeUser([FromQuery] string username)
		{
			var revokeUserResponse = await _mediator.Send(new RevokeUserRequest(username));

			if (!revokeUserResponse.IsSuccess)
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
			=> Ok(await _mediator.Send(new RevokeUsersInRoleRequest(role)));
	}
}
