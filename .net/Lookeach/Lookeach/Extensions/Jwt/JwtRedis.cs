using Lookeach.Application.Services;
using Microsoft.AspNetCore.Authorization;
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
            var endpoint = httpContext.GetEndpoint();
            var metadata = endpoint?.Metadata;
            if (metadata!=null && metadata.FirstOrDefault(x => x.ToString()!.Contains("Authorize Policy"))==null!)
            {
                await _next(httpContext);
                return;
            }
            var token = (httpContext.Request.Headers.Authorization.FirstOrDefault() + "").Split(" ").Last();

            if (string.IsNullOrEmpty(token) || !(await authService.GetTokenValid(token)))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
            await _next(httpContext);
        }
    }
}
