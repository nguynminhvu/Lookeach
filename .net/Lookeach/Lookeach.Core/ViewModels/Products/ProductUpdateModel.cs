using Lookeach.Core.Common.Base;
using Microsoft.AspNetCore.Http;

namespace Lookeach.Core.ViewModels.Products
{
    public class ProductUpdateModel : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public ICollection<IFormFile>? Images { get; set; }
        public ICollection<Guid>? Sizes { get; set; } = new List<Guid>();
        public ICollection<Guid>? Colors { get; set; } = new List<Guid>();
        public ICollection<Guid> Categories { get; set; } = new List<Guid>();
        public Guid BrandId { get; set; }
    }
}
