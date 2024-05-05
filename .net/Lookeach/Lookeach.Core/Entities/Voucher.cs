using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Voucher : Audit<DateTime, Guid>
    {
        public string Code { get; private set; }
        public decimal PriceCondition { get; private set; }
        public decimal MaxPriceOff { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public int SlotEachPerson { get; private set; }
        public int SlotAvailable { get; private set; }
        public bool IsActived { get; private set; }

        private Voucher(Guid id, string code, decimal priceCondition, decimal maxPriceOff, DateTime startTime, DateTime endTime, int slotPerson, int slotAvailable, bool isActive, Guid createdBy, DateTime createdOn, Guid modifiedBy, DateTime modifiedOn)
        {
            Id = id;
            Code = code;
            PriceCondition = priceCondition;
            MaxPriceOff = maxPriceOff;
            StartTime = startTime;
            EndTime = endTime;
            SlotEachPerson = slotPerson;
            SlotAvailable = slotAvailable;
            IsActived = isActive;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            CreatedOn = createdOn;
            ModifiedOn = modifiedOn;
        }

        public static Voucher Create(string code, decimal priceCondition, decimal maxPriceOff, DateTime startTime, DateTime endTime, int slotPerson, int slotAvailable, bool isActive, Guid createdBy)
        {
            return new Voucher(Guid.NewGuid(), code, priceCondition, maxPriceOff, startTime, endTime, slotPerson, slotAvailable, isActive, createdBy, DateTime.Now, createdBy, DateTime.Now);
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
    }
}
