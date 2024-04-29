using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class BrandImage : BaseEntity<Guid>
    {
        public Guid BrandId { get; set; }
        public Guid ImageId { get; set; }
        public Brand Brand { get; set; }
        public Image Image { get; set; }

        protected BrandImage(Guid id, Guid brandId, Guid imageId)
        {
            Id = id;
            BrandId = brandId;
            ImageId = imageId;
        }
        public static BrandImage Create(Guid brandId, Guid imageId)
            => new BrandImage(Guid.NewGuid(), brandId, imageId);

    }
}
