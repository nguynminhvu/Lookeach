using System.ComponentModel.DataAnnotations;

namespace Lookeach.Application.ViewModels.Vouchers
{
    public class VoucherCreateModel
    {
        [Required]
        public decimal PriceCondition { get;  set; }
        [Required]
        public decimal MaxPriceOff { get;  set; }
        [Required]
        public DateTime StartTime { get;  set; }
        [Required]
        public DateTime EndTime { get;  set; }
        [Required]
        public int SlotEachPerson { get;  set; }
        [Required]
        public int SlotAvailable { get;  set; }
    }
}
