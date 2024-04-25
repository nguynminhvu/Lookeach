using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Voucher : Audit<DateTime, Guid>
    {
        public string Code { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsProductVoucher { get; set; }
        public bool IsCategoryVoucher { get; set; }
        public bool IsShopVoucher { get; set; }
        public bool IsActived { get; set; }
    }
}
