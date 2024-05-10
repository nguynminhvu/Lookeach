using Lookeach.Application.Services;
using Lookeach.AuthModels;
using Lookeach.Core.DTOs;
using Lookeach.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text;

namespace Lookeach.AuthOptions
{
    public class ActionRequirementHandler : AuthorizationHandler<ActionRequirement>
    {
        private readonly IServiceScopeFactory _serviceScope;
        private readonly IConfiguration _configuration;
        private readonly IConnectionMultiplexer _connectionMutiple;
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpAccessor;
        private JwtDTO _jwtDTO = new JwtDTO();

        public ActionRequirementHandler(IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache, IConfiguration configuration, IConnectionMultiplexer connectionMultiplexer, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScope = serviceScopeFactory;
            _configuration = configuration;
            _connectionMutiple = connectionMultiplexer;
            _cache = distributedCache;
            _httpAccessor = httpContextAccessor;
            _configuration.GetSection(nameof(JwtDTO)).Bind(_jwtDTO);
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ActionRequirement requirement)
        {
            bool isPassed = false;
            try
            {
                if (!context.User.Identity!.IsAuthenticated || requirement  == null || string.IsNullOrEmpty(requirement.Actions) || context.HasSucceeded)
                {
                    context.Fail();
                    return;
                }
                using (var scope = _serviceScope.CreateScope())
                {
                    var _actionService = scope.ServiceProvider.GetRequiredService<IActionService>();
                    var _permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

                    if (_actionService == null! || _permissionService == null!)
                    {
                        context.Fail();
                        await ToErrorResult("Service error", 500);
                        return;
                    }
                    var claim = await _actionService.GetClaims();
                    if (claim == null!)
                    {
                        context.Fail();
                        await ToErrorResult("Unauthorize", 401);
                        return;
                    }

                    var permissions = await _permissionService.GetPermissionsCache(claim.RoleId);
                    if (permissions?.Any() != true)
                    {
                        context.Fail();
                        await ToErrorResult("Unauthorize", 401);
                        return;
                    }

                    var resource = _httpAccessor.HttpContext?.Request.RouteValues["controller"] + "";

                    if (string.IsNullOrEmpty(resource))
                    {
                        context.Fail();
                        await ToErrorResult("Unauthorize", 401);
                        return;
                    }
                    var permissonActual = permissions.Where(x => x.Resource.Name.ToLower() == resource.ToLower()).FirstOrDefault();

                    if (permissonActual == null!)
                    {
                        context.Fail();
                        await ToErrorResult("Unauthorize", 401);
                        return;
                    }
                    if (await ValidActionResource(_actionService, requirement.Actions, permissonActual.PermissionValue))
                    {
                        context.Succeed(requirement);
                        isPassed = true;
                    }
                }
            }
            catch (Exception ex)
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

        /// <summary>
        /// roleValue mean actual permission
        /// </summary>
        /// <param name="action"></param>
        /// <param name="roleValue"></param>
        /// <returns></returns>
        private async Task<bool> ValidActionResource(IActionService _actionService, string action, int roleValue)
        {
            List<ActionDTO> actionDTOs = await _actionService.GetActionDTOsCache();

            if (actionDTOs?.Any() != true) return false;

            actionDTOs = actionDTOs.OrderBy(x => x.Index).ToList();

            // expected Permission 
            char[] authorizeString = ReverseString(DecimalToBinary(roleValue)).ToArray();

            Dictionary<string, bool> actionKey = new Dictionary<string, bool>();
            int i = 0;
            foreach (var item in authorizeString)
            {
                bool vlue;
                if (item == '0')
                {
                    vlue = false;
                }
                else if (item == '1')
                {
                    vlue = true;
                }
                else
                {
                    return false;
                }
                actionKey.Add(actionDTOs[i].Name.ToLower(), vlue);
                i++;
            }
            if (actionKey.ContainsKey(action.ToLower()))
            {
                return actionKey[action.ToLower()];
            }
            return false;
        }

        private string DecimalToBinary(int decimalNumber)
        {
            if (decimalNumber == 0)
                return "0";

            StringBuilder binary = new StringBuilder();

            while (decimalNumber > 0)
            {
                binary.Insert(0, decimalNumber % 2);
                decimalNumber /= 2;
            }

            return binary.ToString();
        }

        private char[] ReverseString(string str)
        {
            char[] chars = str.ToCharArray();
            int length = str.Length;
            for (int i = 0; i <= length / 2; i++)
            {
                char temp = chars[i];
                chars[i] = chars[length - 1 - i];
                chars[length - 1 - i] = temp;
            }
            return chars;
        }
    }
}
