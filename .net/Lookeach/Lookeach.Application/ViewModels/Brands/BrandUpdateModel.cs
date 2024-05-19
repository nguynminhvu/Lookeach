using Microsoft.AspNetCore.Http;

namespace Lookeach.Application.ViewModels.Brands
{
    public class BrandUpdateModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
