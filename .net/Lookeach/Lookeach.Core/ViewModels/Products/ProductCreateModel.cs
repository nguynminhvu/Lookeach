using Microsoft.AspNetCore.Http;

namespace Lookeach.Core.ViewModels.Products
{
    public class ProductCreateModel
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public ICollection<IFormFile>? Images { get; set; }
        public ICollection<Guid>? Sizes { get; set; }
        public ICollection<Guid>? Colors { get; set; }
        public ICollection<Guid> Categories { get; set; }
        public Guid BrandId { get; set; }
    }
}
