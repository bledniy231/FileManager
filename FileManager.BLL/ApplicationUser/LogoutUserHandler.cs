using FileManager.BLL.TokenService;
using FileManager.Contract.ApplicationUser;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.BLL.ApplicationUser
{
	internal class LogoutUserHandler(ITokenService tokenService) : IRequestHandler<LogoutUserRequest, Unit>
	{
		private readonly ITokenService _tokenService = tokenService;

		public Task<Unit> Handle(LogoutUserRequest request, CancellationToken cancellationToken)
		{
			_tokenService.DeactivateCurrentUserToken();

			return Task.FromResult(Unit.Value);
		}
	}
}
