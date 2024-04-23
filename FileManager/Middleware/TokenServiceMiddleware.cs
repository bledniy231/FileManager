using FileManager.BLL.TokenService;
using System.Net;

namespace FileManager.Middleware
{
	public class TokenServiceMiddleware(ITokenService tokenManager) : IMiddleware
	{
		private readonly ITokenService _tokenService = tokenManager;

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			if (_tokenService.IsCurrentUserActiveToken())
			{
				await next(context);

				return;
			}
			context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
		}
	}
}
