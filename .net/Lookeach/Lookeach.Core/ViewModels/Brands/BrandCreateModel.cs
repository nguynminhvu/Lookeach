using Microsoft.AspNetCore.Http;

namespace Lookeach.Core.ViewModels.Brands
{
    public class BrandCreateModel
    {
        public string Name { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
