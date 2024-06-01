using Lookeach.Application.DTOs;
using Lookeach.Application.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace Lookeach.Extensions.AuthResource
{
    public class AuthResourceOption
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;
        private readonly RedisConstantDTO _redisConstantDTO = new();
        public AuthResourceOption(RequestDelegate requestDelegate, IDistributedCache distributedCache, IConfiguration configuration)
        {
            _requestDelegate = requestDelegate;
            _distributedCache = distributedCache;
            _configuration = configuration;
            _configuration.GetSection(nameof(RedisConstantDTO)).Bind(_redisConstantDTO);
        }

        public async Task Invoke(HttpContext context, IPermissionService permissionService,IActionService actionService)
        {
            var permission = await _distributedCache.GetStringAsync(_redisConstantDTO.PermissionResource);
            if (permission == null)
            {
                await permissionService.SavePermissionCache();
            }

            var actions = await _distributedCache.GetStringAsync(_redisConstantDTO.ActionsResource);
            if (actions == null!)
            {
                await actionService.SaveActionsCache();
            }

            await _requestDelegate(context);
        }
    }
}
