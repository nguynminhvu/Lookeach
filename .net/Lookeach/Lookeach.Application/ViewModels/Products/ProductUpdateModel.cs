using Lookeach.Core.Common.Base;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Lookeach.Application.ViewModels.Products
{
    public class ProductUpdateModel : BaseEntity<Guid>
    {
        [Required(ErrorMessage = "Product Name is required")]
        [MinLength(5, ErrorMessage = "Product Name at least 5 character")]
        [MaxLength(50, ErrorMessage = "Product Name at most 50 character")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product Price is required")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Product Price is required")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Product Description is required")]
        [MinLength(50, ErrorMessage = "Product Description at least 50 character")]
        [MaxLength(1000, ErrorMessage = "Product Description at most 1000 character")]
        public string Description { get; set; }
        public ICollection<IFormFile>? Images { get; set; }
        public ICollection<Guid>? Sizes { get; set; } = new List<Guid>();
        public ICollection<Guid>? Colors { get; set; } = new List<Guid>();
        public ICollection<Guid> Categories { get; set; } = new List<Guid>();
        public Guid BrandId { get; set; }
    }
}
