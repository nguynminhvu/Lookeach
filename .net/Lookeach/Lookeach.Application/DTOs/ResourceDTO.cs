using Lookeach.Core.Common.Base;

namespace Lookeach.Application.DTOs
{
    public class ResourceDTO : BaseEntity<Guid>
    {
        public string Name { get; set; }
    }
}
