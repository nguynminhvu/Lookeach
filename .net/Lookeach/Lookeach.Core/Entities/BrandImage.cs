using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class BrandImage : BaseEntity<Guid>
    {
        public Guid BrandId { get; set; }
        public Guid ImageId { get; set; }
        public Brand Brand { get; set; }
        public Image Image { get; set; }
    }
}
