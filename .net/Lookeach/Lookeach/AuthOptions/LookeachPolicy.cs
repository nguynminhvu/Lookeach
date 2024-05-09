using Lookeach.AuthModels;
using Lookeach.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Lookeach.AuthOptions
{
    public class LookeachPolicy : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public LookeachPolicy(IOptions<AuthorizationOptions> options)
        {
            options.Value.InvokeHandlersAfterFailure = false;
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build());
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
            => FallbackPolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (string.IsNullOrWhiteSpace(policyName))
            {
                return FallbackPolicyProvider.GetPolicyAsync(policyName);
            }
            var policyTokens = policyName.Split(';', StringSplitOptions.RemoveEmptyEntries);

            if (policyTokens?.Any() != true)
            {
                return FallbackPolicyProvider.GetPolicyAsync(policyName);
            }
            var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);

            foreach (var item in policyTokens)
            {
                var policySplit = item.Split('$', StringSplitOptions.RemoveEmptyEntries);

                // Authorize with 2 attributr
                if (policySplit != null! && policySplit.Count() == 2)
                {
                    IAuthorizationRequirement requirement = policySplit[0] switch
                    {
                        LookeachAuthorizeAttribute.RolesGroup => new RoleRequirement(policySplit[1]),
                        LookeachAuthorizeAttribute.ActionsGroup => new ActionRequirement(policySplit[1]),
                        _ => null!
                    };

                    if (requirement == null!) return FallbackPolicyProvider.GetPolicyAsync(policyName);
                    policy.AddRequirements(requirement);
                }
            }
            return Task.FromResult(policy.Build())!;
        }
    }
}
