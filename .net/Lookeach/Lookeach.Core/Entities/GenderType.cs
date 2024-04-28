using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class GenderType:Audit<DateTime,Guid>
    {
        public string Name { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
