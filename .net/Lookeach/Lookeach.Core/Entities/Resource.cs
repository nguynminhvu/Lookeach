using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Resource : Audit<DateTime, Guid>
    {
        public string Name { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}
