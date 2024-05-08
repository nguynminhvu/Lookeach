using Lookeach.AuthModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Lookeach.AuthOptions
{
    public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpAccessor;

        public RoleRequirementHandler(IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache)
        {
            _cache = distributedCache;
            _httpAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
        private async bool ValidActionResource()
        {
            var actionString = await _cache.GetStringAsync("ActionResource");
            if(string.IsNullOrEmpty(actionString)) return false;
            var action=JsonConvert.DeserializeObject<List<>>>(actionString);
        }
    }
}
