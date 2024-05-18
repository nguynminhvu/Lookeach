using Lookeach.Core.Entities;

namespace Lookeach.Application.ViewModels.Brands
{
    public class BrandViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<BrandImageViewModel>? BrandImages { get; set; }
    }
}
