using MediatR;
using PianoMentor.BLL.Services.TokenService;
using PianoMentor.Contract.ApplicationUser;

namespace PianoMentor.BLL.UseCases.ApplicationUser
{
	internal class LogoutUserHandler(ITokenService tokenService) : IRequestHandler<LogoutUserRequest, Unit>
	{
		public Task<Unit> Handle(LogoutUserRequest request, CancellationToken cancellationToken)
		{
			tokenService.DeactivateCurrentUserToken();

			return Task.FromResult(Unit.Value);
		}
	}
}
