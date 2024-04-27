using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class ProductSourcing : Audit<DateTime, Guid>
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public ICollection<ProductSourcingImage>? ProductSourcingImages { get; set; }
        public ICollection<Size>? Sizes { get; set; }
        public ICollection<Color>? Colors { get; set; }
        public ICollection<Category> Categories { get; set; }
        public Guid BrandId { get; set; }
        public Brand Brand { get; set; }
        public int Version { get; set; }
    }
}
