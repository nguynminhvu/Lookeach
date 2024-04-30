using Lookeach.Core.Common.Base;
using Microsoft.AspNetCore.Http;

namespace Lookeach.Core.ViewModels.Categories
{
    public class CategoryUpdateModel : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
