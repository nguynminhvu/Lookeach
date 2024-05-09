using Lookeach.Application.Services;
using Lookeach.AuthModels;
using Lookeach.Core.DTOs;
using Lookeach.Infrastructure.Services;
using Lookeach.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text;

namespace Lookeach.AuthOptions
{
    public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly IServiceScopeFactory _serviceScope;
        private readonly IConfiguration _configuration;
        private readonly IConnectionMultiplexer _connectionMutiple;
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpAccessor;
        private JwtDTO _jwtDTO = new JwtDTO();

        public RoleRequirementHandler(IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache, IConfiguration configuration, IConnectionMultiplexer connectionMultiplexer, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScope = serviceScopeFactory;
            _configuration = configuration;
            _connectionMutiple = connectionMultiplexer;
            _cache = distributedCache;
            _httpAccessor = httpContextAccessor;
            _configuration.GetSection(nameof(JwtDTO)).Bind(_jwtDTO);
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            bool isPassed = false;
            try
            {
                if (context.User.Identity?.IsAuthenticated != true || context.HasSucceeded || requirement == null! || string.IsNullOrEmpty(requirement.Roles))
                {
                    context.Fail();
                    await ToErrorResult("Unauthorized", 401);
                    return;
                }
                using (var scope = _serviceScope.CreateScope())
                {
                    var _actionService = scope.ServiceProvider.GetRequiredService<IActionService>();
                    if (_actionService == null!)
                    {
                        context.Fail();
                        await ToErrorResult("Service error", 500);
                        return;
                    }
                    var entity = _httpAccessor.HttpContext?.Request.RouteValues["controller"]?.ToString();
                    if (string.IsNullOrEmpty(entity))
                    {
                        context.Fail();
                        await ToErrorResult("Not found your action", 401);
                        return;
                    }

                    entity = entity.ToLower().Replace("controller", "");
                    entity = entity.Replace(entity[0].ToString(), entity[0].ToString().ToUpper());
                    var token = (_httpAccessor.HttpContext?.Request.Headers["Authorization"].First() + "").Split(' ').Last();
                    var claim = await _actionService.GetClaims(token);

                    if (claim == null! || claim.RoleId == Guid.Empty)
                    {
                        context.Fail();
                        await ToErrorResult("Unauthorize", 401);
                        return;
                    }
                    if (claim.Role.ToLower() == requirement.Roles.ToLower())
                    {
                        context.Succeed(requirement);
                        isPassed = true;
                    }
                }
            }
            catch
            {

            }
            if (!isPassed)
            {
                await ToErrorResult("Your role doesn't allow you to access this endpoint in a manner you trying to access it", 403);
                context.Fail();
            }
        }

        private async Task ToErrorResult(string msgError, int statusCode)
        {
            if (_httpAccessor.HttpContext!.Response.HasStarted)
            {
                ResponseBase<string> response = new ResponseBase<string>
                {
                    Error = new ErrorViewModel
                    {
                        Code = statusCode,
                        Message = msgError
                    },
                    IsFailure = true,
                    IsSuccess = false,
                    Value = null!
                };

                _httpAccessor.HttpContext.Response.ContentType = "application/json";
                _httpAccessor.HttpContext.Response.StatusCode = statusCode;
                await _httpAccessor.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }
}
