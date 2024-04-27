using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class ProductImage : BaseEntity<Guid>
    {
        public Guid ProductId { get; set; }
        public Guid ImageId { get; set; }

        public Product Product { get; set; }
        public Image Image { get; set; }
    }
}
