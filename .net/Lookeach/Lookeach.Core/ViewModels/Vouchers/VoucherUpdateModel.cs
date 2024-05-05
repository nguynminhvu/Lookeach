using Lookeach.Core.Common.Base;

namespace Lookeach.Core.ViewModels.Vouchers
{
    public class VoucherUpdateModel : BaseEntity<Guid>
    {
        public decimal PriceCondition { get; private set; }
        public decimal MaxPriceOff { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public int SlotEachPerson { get; private set; }
        public int SlotAvailable { get; private set; }
    }
}
