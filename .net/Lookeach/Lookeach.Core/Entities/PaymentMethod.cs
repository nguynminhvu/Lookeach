using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class PaymentMethod : Audit<DateTime, Guid>
    {
        public string Name { get; private set; }
        public ICollection<Order> Orders { get; private set; }

        private PaymentMethod()
        {

        }
        private PaymentMethod(Guid id, string name, Guid createdBy, DateTime createdOn, Guid modifiedBy, DateTime modifiedOn)
        {
            Id = id;
            if (string.IsNullOrEmpty(name)) throw new Exception("Name required");
            Name = name;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            ModifiedOn = modifiedOn;
            CreatedOn = createdOn;
        }
        public static PaymentMethod Create(string name, Guid createdBy)
        {
            return new PaymentMethod(Guid.NewGuid(), name, createdBy, DateTime.Now, createdBy, DateTime.Now);
        }

        public void Update(string name, Guid modifiedBy)
        {
            Name = name;
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTime.Now;
        }
    }
}
