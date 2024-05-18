using Lookeach.Application.ViewModels.Images;
using Lookeach.Core.Common.Base;

namespace Lookeach.Application.ViewModels.Categories
{
    public class CategoryImageViewModel:BaseEntity<Guid>
    {
        public Guid Id { get; set; }
        public ImageViewModel Image { get; set; }

    }
}
