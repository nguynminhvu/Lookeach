using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Permission : BaseEntity<Guid>
    {
        public Guid RoleId { get; set; }
        public Guid ResourceId { get; set; }
        public int PermissionValue { get; set; }
        public Role Role { get; set; }
        public Resource Resource { get; set; }
    }
}
