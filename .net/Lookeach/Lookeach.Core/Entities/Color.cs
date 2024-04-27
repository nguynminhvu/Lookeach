using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Color:Audit<DateTime,Guid>
    {
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
