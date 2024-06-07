using Lookeach.Application.Services;
using Lookeach.Infrastructure.Services;
using Lookeach.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Net.Http;

namespace Lookeach.Extensions.AuthAttribute
{
    public class Authorize : IAsyncAuthorizationFilter
    {
        private readonly IAuthService _authService;

        public Authorize(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Case token valid and user re-use token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var auth = context.ActionDescriptor.EndpointMetadata.Any(x => x.GetType() == typeof(AuthorizeAttribute));
            if (auth)
            {
                var token = (context.HttpContext.Request.Headers.Authorization.FirstOrDefault() + "").Split(" ").Last();

                if (string.IsNullOrEmpty(token) || !(await _authService.GetTokenValid(token)))
                {
                    ResponseViewModel responseView = new ResponseViewModel
                    {
                        Error = new ErrorViewModel
                        {
                            Code = (int)HttpStatusCode.BadRequest,
                            Message = "Token invalid"
                        },
                        IsFailure = true,
                        IsSuccess = false,
                        Value = null!
                    };

                    context.Result = new ObjectResult(responseView)
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                    return;
                }
            }
        }
    }
}
