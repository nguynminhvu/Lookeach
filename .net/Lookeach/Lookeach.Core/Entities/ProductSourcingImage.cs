using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class ProductSourcingImage : BaseEntity<Guid>
    {
        public Guid ProductSourcingId {private get; set; }
        public Guid ImageId { private get; set; }
        public ProductSourcing ProductSourcing { private get; set; }
        public Image Image { private get; set; }

        private ProductSourcingImage()
        {

        }
        protected ProductSourcingImage(Guid id, Guid productSourcingId, Guid imageId)
        {
            Id = id;
            ProductSourcingId = productSourcingId;
            ImageId = imageId;
        }

        public static ProductSourcingImage Create(Guid productSourcingId, Guid imageId)
        {
            return new ProductSourcingImage(Guid.NewGuid(), productSourcingId, imageId);
        }
    }
}
