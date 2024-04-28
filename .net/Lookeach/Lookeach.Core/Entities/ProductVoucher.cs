using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class ProductVoucher:Audit<DateTime,Guid>
    {
        public Guid VoucherId { get; set; }
        public Voucher Voucher { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
