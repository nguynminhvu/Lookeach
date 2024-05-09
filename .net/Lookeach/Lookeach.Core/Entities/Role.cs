using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Role : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}
