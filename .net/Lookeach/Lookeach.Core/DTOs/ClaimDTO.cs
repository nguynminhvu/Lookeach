using Lookeach.Core.Common.Base;
using Lookeach.Core.Entities;

namespace Lookeach.Core.DTOs
{
    public class ClaimDTO : BaseEntity<Guid>
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public Guid RoleId { get; set; }
    }
}
