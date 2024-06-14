using System.Net;
using PianoMentor.BLL.Services.TokenService;

namespace PianoMentor.Middleware
{
	public class TokenServiceMiddleware(ITokenService tokenManager) : IMiddleware
	{
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			if (tokenManager.IsCurrentUserActiveToken())
			{
				await next(context);

				return;
			}
			context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
		}
	}
}
