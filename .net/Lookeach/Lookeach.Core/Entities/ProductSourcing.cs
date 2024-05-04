using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class ProductSourcing : Audit<DateTime, Guid>
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Sold { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public ICollection<ProductSourcingImage>? ProductSourcingImages { get; set; }
        public ICollection<Size>? Sizes { get; set; }
        public ICollection<Color>? Colors { get; set; }
        public ICollection<Category> Categories { get; set; }
        public Guid BrandId { get; set; }
        public Brand Brand { get; set; }
        public int Version { get; set; }

        protected ProductSourcing(Guid id, string name, int quantity, int sold, decimal price, string description, List<Size> sizes, List<Color> colors, List<Category> categories, Guid brandId, Guid createdBy, DateTime createdOn, Guid modifiedBy, DateTime modifiedOn, int version, Guid productId)
        {
            Id = id;
            Name = name;
            if (quantity <= 0) throw new Exception("Quantity greater than 0 please");
            Quantity = quantity;
            Sold = sold;
            if (price <= 0) throw new Exception("Price greater than 0 please");
            Price = price;
            Description = description;
            BrandId = brandId;
            if (sizes.Count <= 0)
            {
                throw new Exception("Sizes required");
            }
            if (colors.Count <= 0)
            {
                throw new Exception("Colors required");
            }
            if (categories.Count <= 0)
            {
                throw new Exception("Categories required");
            }
            Sizes = sizes;
            Colors = colors;
            Categories = categories;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            CreatedOn = createdOn;
            ModifiedOn = modifiedOn;
            Version= version;
            ProductId= productId;
        }

        public static ProductSourcing CreateProductSourcing(string name, int quantity, decimal price, string description, List<Size> sizes, List<Color> colors, List<Category> categories, Guid brandId, Guid createdBy,int version, Guid productId)
        {
            return new ProductSourcing(Guid.NewGuid(), name, quantity, 0, price, description, sizes, colors, categories, brandId, createdBy, DateTime.Now, createdBy, DateTime.Now, version, productId);
        }
      
    }
}
