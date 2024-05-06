using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class VoucherSourcing : Audit<DateTime, Guid>
    {
        public Guid UserId { get; private set; }
        public Guid VoucherId { get; private set; }

        public User User { get; private set; }
        public Voucher Voucher { get; private set; }

        private VoucherSourcing()
        {

        }
        protected VoucherSourcing(Guid id, Guid userId, Guid voucherId, Guid createdBy, DateTime createdOn, Guid modifiedBy, DateTime modifiedOn)
        {
            Id = id;
            UserId = userId;
            VoucherId = voucherId;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            CreatedOn = createdOn;
            ModifiedOn = modifiedOn;
        }

        public static VoucherSourcing Create(Guid userId, Guid voucherId, Guid createdBy)
        {
            return new VoucherSourcing(Guid.NewGuid(), userId, voucherId, createdBy, DateTime.Now, createdBy, DateTime.Now);
        }
    }
}
