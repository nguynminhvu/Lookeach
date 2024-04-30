using Lookeach.Core.Common.Base;
using Lookeach.Core.ViewModels.Images;

namespace Lookeach.Core.ViewModels.Categories
{
    public class CategoryImageViewModel:BaseEntity<Guid>
    {
        public Guid Id { get; set; }
        public ImageViewModel Image { get; set; }

    }
}
