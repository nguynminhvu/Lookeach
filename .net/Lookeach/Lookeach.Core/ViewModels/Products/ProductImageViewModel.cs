using Lookeach.Core.Common.Base;
using Lookeach.Core.ViewModels.Images;

namespace Lookeach.Core.ViewModels.Products
{
    public class ProductImageViewModel : BaseEntity<Guid>
    {
        public Guid ImageId { get; set; }

        public ImageViewModel Image { get; set; }
    }
}
