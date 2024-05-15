using Lookeach.Core.Common.Base;

namespace Lookeach.Application.DTOs
{
    public class PermissionDTO : BaseEntity<Guid>
    {
        public Guid RoleId { get; set; }
        public Guid ResourceId { get; set; }
        public int PermissionValue { get; set; }
        public RoleDTO Role { get; set; }
        public ResourceDTO Resource { get; set; }
    }
}
