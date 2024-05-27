using Microsoft.AspNetCore.Http;

namespace Lookeach.Application.Features.Brands.Models
{
    public class BrandCreateModel
    {
        public string Name { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
