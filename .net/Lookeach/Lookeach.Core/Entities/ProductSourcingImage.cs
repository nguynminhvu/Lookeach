using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class ProductSourcingImage : BaseEntity<Guid>
    {
        public Guid ProductSourcingId { get; set; }
        public Guid ImageId { get; set; }
        public ProductSourcing ProductSourcing { get; set; }
        public Image Image { get; set; }
    }
}
