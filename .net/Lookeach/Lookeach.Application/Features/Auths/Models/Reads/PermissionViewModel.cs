using Lookeach.Core.Common.Base;

namespace Lookeach.Application.Features.Auths.Models.Reads
{
    public class PermissionViewModel : BaseEntity<Guid>
    {
        public int PermissionValue { get; set; }
        public RoleViewModel Role { get; set; }
        public ResourceViewModel Resource { get; set; }
    }
}
