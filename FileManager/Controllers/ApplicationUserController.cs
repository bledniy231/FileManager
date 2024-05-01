﻿using FileManager.Certificates;
using FileManager.Contract.ApplicationUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
		public async Task<ActionResult<AuthUserResponse>> Login([FromBody] AuthUserRequest request)
		{
			var userAuthResponse = await _mediator.Send(request);

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
			var userRegisterResponse = await _mediator.Send(request);
			
			if (!userRegisterResponse.Errors.IsNullOrEmpty())
			{
				return BadRequest(userRegisterResponse.Errors);
			}
			
			return await Login(new AuthUserRequest(userRegisterResponse.Email, userRegisterResponse.Password));
		}

		[Authorize]
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<RefreshUserTokensResponse>> RefreshUserTokens([FromBody] RefreshUserTokensRequest request)
		{
			var userRefreshResponse = await _mediator.Send(request);

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
			var revokeUserResponse = await _mediator.Send(new RevokeUserRequest(username));

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
			=> Ok(await _mediator.Send(new RevokeUsersInRoleRequest(role)));

		[Authorize]
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Logout()
		{
			await _mediator.Send(new LogoutUserRequest());

			return NoContent();
		}

		[HttpGet]
		public Task<IActionResult> CreateServerCertificate()
		{
			var serverCertificateManager = new FileManagerServerCertificateManager();
			serverCertificateManager.Create();
			return Task.FromResult<IActionResult>(Ok());
		}

		[HttpGet]
		public Task<IActionResult> CreateClientCertificate()
		{
			var clientCertificateManager = new PianoMentorClientCertificateManager();
			clientCertificateManager.Create();
			return Task.FromResult<IActionResult>(Ok());
		}
	}
}
