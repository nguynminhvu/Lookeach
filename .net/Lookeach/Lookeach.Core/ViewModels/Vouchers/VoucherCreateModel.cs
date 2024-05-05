using System.ComponentModel.DataAnnotations;

namespace Lookeach.Core.ViewModels.Vouchers
{
    public class VoucherCreateModel
    {
        [Required]
        public decimal PriceCondition { get; private set; }
        [Required]
        public decimal MaxPriceOff { get; private set; }
        [Required]
        public DateTime StartTime { get; private set; }
        [Required]
        public DateTime EndTime { get; private set; }
        [Required]
        public int SlotEachPerson { get; private set; }
        [Required]
        public int SlotAvailable { get; private set; }
    }
}
