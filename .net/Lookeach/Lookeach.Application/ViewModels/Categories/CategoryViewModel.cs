using Lookeach.Core.Common.Base;

namespace Lookeach.Application.ViewModels.Categories
{
    public class CategoryViewModel : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public List<CategoryImageViewModel> CategoryImages { get; set; }

    }
}
