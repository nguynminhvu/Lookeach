using Lookeach.AuthOptions;
using Lookeach.Extensions.AuthResource;
using Microsoft.AspNetCore.Authorization;

namespace Lookeach.Extensions.Injects
{
    public static class DI
    {
        public static void InjectDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAuthorizationPolicyProvider, LookeachPolicy>();
            services.AddTransient<IAuthorizationHandler, ActionRequirementHandler>();
          //  services.AddTransient<IAuthorizationHandler, RoleRequirementHandler>();
        }

        public static void UseABAC(this IApplicationBuilder application)
        {
            application.UseMiddleware<AuthResourceOption>();
        }
    }
}
