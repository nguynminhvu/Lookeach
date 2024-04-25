using Lookeach.Application.Services;
using System.Net;

namespace Lookeach.Extensions.Jwt
{
    public class JwtRedis
    {
        private readonly RequestDelegate _next;

        public JwtRedis(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IAuthService authService)
        {
            var token = httpContext.Request.Headers.Authorization.First()!.Split(" ").Last();

            if (string.IsNullOrEmpty(token) || !(await authService.GetTokenValid(token)))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
            await _next(httpContext);
        }
    }
}
