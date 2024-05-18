using Lookeach.Application.ViewModels.Brands;
using Lookeach.Application.ViewModels.Categories;
using Lookeach.Application.ViewModels.Colors;
using Lookeach.Application.ViewModels.Sizes;
using Lookeach.Core.Common.Base;

namespace Lookeach.Application.ViewModels.Products
{
    public class ProductViewModel : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public ICollection<ProductImageViewModel>? ProductImages { get; set; }
        public ICollection<SizeViewModel>? Sizes { get; set; }
        public ICollection<ColorViewModel>? Colors { get; set; }
        public ICollection<CategoryViewModel> Categories { get; set; }
        public Guid BrandId { get; set; }
        public BrandViewModel Brand { get; set; }
    }
}
