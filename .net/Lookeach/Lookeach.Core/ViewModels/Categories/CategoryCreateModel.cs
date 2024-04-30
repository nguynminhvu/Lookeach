using Microsoft.AspNetCore.Http;

namespace Lookeach.Core.ViewModels.Categories
{
    public  class CategoryCreateModel
    {
        public string Name { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
