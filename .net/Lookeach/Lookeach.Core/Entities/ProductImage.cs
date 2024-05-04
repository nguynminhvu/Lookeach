using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class ProductImage : BaseEntity<Guid>
    {
        public Guid ProductId { get; private set; }
        public Guid ImageId { get; private set; }

        public Product Product { get; set; }
        public Image Image { get; set; }
        protected ProductImage(Guid id, Guid productId, Guid imageId)
        {
            Id = id;
            ProductId = productId;
            ImageId = imageId;
        }

        public static ProductImage Create(Guid productId, Guid imageId)
        {
            return new ProductImage(Guid.NewGuid(), productId, imageId);
        }
    }
}
