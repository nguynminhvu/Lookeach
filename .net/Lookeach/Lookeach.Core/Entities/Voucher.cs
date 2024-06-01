using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Voucher : Audit<DateTime, Guid>
    {
        public string Code { get; private set; }
        public decimal PriceCondition { get; private set; }
        public decimal MaxPriceOff { get; private set; }
        public double LevelOff { get; set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public int SlotEachPerson { get; private set; }
        public int SlotAvailable { get; private set; }
        public int CurrentSlot { get; private set; }
        public bool IsActived { get; private set; }
        public bool IsSystem { get; private set; }
        public ICollection<Order> Orders { get; private set; }
        private Voucher()
        {

        }
        private Voucher(Guid id, string code, decimal priceCondition, decimal maxPriceOff, DateTime startTime, DateTime endTime, int slotPerson, int slotAvailable, int currentSlot, bool isActive, bool isSystem)
        {
            Id = id;
            Code = code;
            PriceCondition = priceCondition;
            MaxPriceOff = maxPriceOff;
            StartTime = startTime;
            EndTime = endTime;
            SlotEachPerson = slotPerson;
            SlotAvailable = slotAvailable;
            CurrentSlot = currentSlot;
            IsActived = isActive;
            IsSystem = isSystem;
        }

        public static Voucher Create(string code, decimal priceCondition, decimal maxPriceOff, DateTime startTime, DateTime endTime, int slotPerson, int slotAvailable, bool isActive,bool isSystem)
        {
            return new Voucher(Guid.NewGuid(), code, priceCondition, maxPriceOff, startTime, endTime, slotPerson, slotAvailable, 0, isActive, isSystem);
        }

        public void UpdateVoucher(decimal priceCondition, decimal maxPriceOff, DateTime startTime, DateTime endTime, int slotPerson, int slotAvailable, Guid modified)
        {
            PriceCondition = priceCondition;
            MaxPriceOff = maxPriceOff;
            StartTime = startTime;
            EndTime = endTime;
            SlotEachPerson = slotPerson;
            SlotAvailable = slotAvailable;
            ModifiedBy = modified;
            ModifiedOn = DateTime.Now;
        }

        public void DeleteVoucher(bool isActived)
        {
            IsActived = isActived;
        }

        public void UpdateCurrentSlot(int currentSlot, Guid modifiedBy)
        {
            CurrentSlot = currentSlot;
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTime.Now;
        }
    }
}
