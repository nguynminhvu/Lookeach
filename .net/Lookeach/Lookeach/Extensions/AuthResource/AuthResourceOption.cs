using Lookeach.Application.Services;
using Lookeach.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace Lookeach.Extensions.AuthResource
{
    public class AuthResourceOption
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly IDistributedCache _distributedCache;
        private readonly IRoleService _roleService;

        public AuthResourceOption(RequestDelegate requestDelegate, IDistributedCache distributedCache, IRoleService roleService)
        {
            _requestDelegate = requestDelegate;
            _distributedCache = distributedCache;
            _roleService = roleService;
        }

        public async Task Invoke(HttpContext context)
        {

        }
    }
}
