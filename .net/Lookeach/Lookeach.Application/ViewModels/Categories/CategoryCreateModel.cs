using Microsoft.AspNetCore.Http;

namespace Lookeach.Application.ViewModels.Categories
{
    public  class CategoryCreateModel
    {
        public string Name { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
