using Microsoft.AspNetCore.Authorization;

namespace Lookeach.AuthModels
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string Roles { get; set; }
        public RoleRequirement(string roles)
        {
            Roles=roles;
        }
    }
}
