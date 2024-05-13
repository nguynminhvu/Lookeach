using Lookeach.Core.Common.Base;
using Lookeach.Core.Entities;

namespace Lookeach.Application.DTOs
{
    public class RoleDTO : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public int ActionValue { get; set; }
        public ICollection<ResourceDTO> Resources { get; set; }
    }
}
