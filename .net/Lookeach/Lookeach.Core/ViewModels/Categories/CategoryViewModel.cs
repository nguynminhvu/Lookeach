using Lookeach.Core.Common.Base;

namespace Lookeach.Core.ViewModels.Categories
{
    public class CategoryViewModel : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public List<CategoryImageViewModel> CategoryImages { get; set; }

    }
}
