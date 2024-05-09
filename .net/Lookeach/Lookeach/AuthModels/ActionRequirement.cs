using Microsoft.AspNetCore.Authorization;

namespace Lookeach.AuthModels
{
    public class ActionRequirement : IAuthorizationRequirement
    {
        public string Actions { get; set; }
        public ActionRequirement(string action)
        {
            Actions = action;
        }
    }
}
