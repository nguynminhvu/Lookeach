using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Role : BaseEntity<Guid>
    {
        public string Name { get; set; }
    }
}
