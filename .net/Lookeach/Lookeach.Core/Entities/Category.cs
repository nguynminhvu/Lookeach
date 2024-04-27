using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Category : Audit<DateTime, Guid>
    {
        public string Name { get; set; }
        public ICollection<CategoryImage>? CategoryImages { get; set; }
        public ICollection<GenderType>? GenderTypes { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<ProductSourcing> ProductSourcings { get; set; }
    }
}
