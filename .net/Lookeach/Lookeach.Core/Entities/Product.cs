using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Product : Audit<DateTime, Guid>
    {
        public string Name { get; private set; }
        public int Quantity { get; private set; }
        public int Sold { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }

        public ICollection<ProductImage>? ProductImages { get; private set; }
        public ICollection<Size>? Sizes { get; private set; }
        public ICollection<Color>? Colors { get; private set; }
        public ICollection<Category> Categories { get; private set; }
        public Guid BrandId { get; private set; }
        public Brand Brand { get; private set; }
        public bool IsActived { get; private set; }

        private Product()
        {

        }
        protected Product(Guid id, string name, int quantity, int sold, decimal price, string description, List<Size> sizes, List<Color> colors, List<Category> categories, Guid brandId, Guid createdBy, DateTime createdOn, Guid modifiedBy, DateTime modifiedOn)
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
            IsActived = true;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            CreatedOn = createdOn;
            ModifiedOn = modifiedOn;
        }

        public static Product CreateProduct(string name, int quantity, decimal price, string description, List<Size> sizes, List<Color> colors, List<Category> categories, Guid brandId, Guid createdBy)
        {
            return new Product(Guid.NewGuid(), name, quantity, 0, price, description, sizes, colors, categories, brandId, createdBy, DateTime.Now, createdBy, DateTime.Now);
        }

        public void UpdateProduct(string name, int quantity, decimal price, string description,  List<Size> sizes, List<Color> colors, List<Category> categories, Guid brandId, Guid modifiedBy)
        {
            Name = name;
            Quantity = quantity;
            if (quantity <= 0) throw new Exception("Quantity greater than 0 please");
            Quantity = quantity;
            if (price <= 0) throw new Exception("Price greater than 0 please");
            Price = price;
            Description = description;
            if (sizes.Count > 0)
            {
                Sizes = sizes;
            }
            if (colors.Count > 0)
            {
                Colors = colors;
            }
            if (categories.Count > 0)
            {
                Categories = categories;
            }
            BrandId = brandId;
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTime.Now;
        }

        public void DeleteProduct(bool status, Guid modifiedBy)
        {
            IsActived = status;
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTime.Now;
        }
    }
}
