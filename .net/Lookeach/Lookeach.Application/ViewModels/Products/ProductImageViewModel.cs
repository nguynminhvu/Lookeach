using Lookeach.Application.ViewModels.Images;
using Lookeach.Core.Common.Base;

namespace Lookeach.Application.ViewModels.Products
{
    public class ProductImageViewModel : BaseEntity<Guid>
    {
        public Guid ImageId { get; set; }

        public ImageViewModel Image { get; set; }
    }
}
