using Lookeach.Core.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Lookeach.AuthOptions
{
    public class LookeachAuthorizeAttribute : AuthorizeAttribute
    {
      //  public const string RolesGroup = "Roles";
        public const string ActionsGroup = "Actions";

   //     public RoleName[] RoleNames { get; set; }
        public string[] Actions { get; set; }

        // no need RoleName[] roleNames right now
        public LookeachAuthorizeAttribute( string[] actions)
        {
       //     RoleNames = roleNames;
            Actions = actions;

         //   Policy += $"{RolesGroup}${string.Join("|", RoleNames)};";
            Policy += $"{ActionsGroup}${string.Join("|", Actions)};";
        }
    }
}
