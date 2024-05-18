using Lookeach.Core.Common.Base;

namespace Lookeach.Application.ViewModels.Vouchers
{
    public class VoucherViewModel : BaseEntity<Guid>
    {
        public string Code { get; private set; }
        public decimal PriceCondition { get; private set; }
        public decimal MaxPriceOff { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public int SlotEachPerson { get; private set; }
        public int SlotAvailable { get; private set; }
        public bool IsActived { get; private set; }
    }
}
