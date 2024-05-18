using Lookeach.Core.Common.Base;

namespace Lookeach.Application.ViewModels.Vouchers
{
    public class VoucherUpdateModel : BaseEntity<Guid>
    {
        /// <summary>
        /// Minimum price condition to apply voucher
        /// </summary>
        public decimal PriceCondition { get;  set; }
        /// <summary>
        /// Max price can sale off when apply in order
        /// </summary>
        public decimal MaxPriceOff { get;  set; }
        public DateTime StartTime { get;  set; }
        public DateTime EndTime { get;  set; }
        /// <summary>
        /// Slot available of each people in 1 voucher
        /// </summary>
        public int SlotEachPerson { get;  set; }
        /// <summary>
        /// Slot available of voucher
        /// </summary>
        public int SlotAvailable { get;  set; }
    }
}
