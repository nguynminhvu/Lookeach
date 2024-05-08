using Lookeach.Core.Common.Base;

namespace Lookeach.Core.DTOs
{
    public class ResourceDTO : BaseEntity<Guid>
    {
        public string Name { get; set; }
    }
}
