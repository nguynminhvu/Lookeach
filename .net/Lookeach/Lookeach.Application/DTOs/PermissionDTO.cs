using Lookeach.Core.ViewModels.Resources;
using Lookeach.Core.ViewModels.Roles;

namespace Lookeach.Application.DTOs
{
    public class PermissionDTO
    {
        public Guid RoleId { get; set; }
        public List<ResourceViewModel> Resources { get; set; }
        public RoleViewModel Role { get; set; }
    }
}
