using Lookeach.Core.Common.Base;
using Lookeach.Core.Entities;
using Lookeach.Core.ViewModels.Brands;
using Lookeach.Core.ViewModels.Categories;
using Lookeach.Core.ViewModels.Colors;
using Lookeach.Core.ViewModels.Sizes;

namespace Lookeach.Core.ViewModels.Products
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
