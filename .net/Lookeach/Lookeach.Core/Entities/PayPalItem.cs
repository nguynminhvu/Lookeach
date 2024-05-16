using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class PayPalItem : Audit<DateTime, Guid>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Quantity { get; private set; }
        public string CurrencyCode { get; private set; }
        public decimal Value { get; private set; }
        public Guid PaypalOrderId { get; private set; }

        public PayPalOrder PaypalOrder { get; set; }

        private PayPalItem()
        {

        }
        protected PayPalItem(Guid id, string name, string description, int quantity, string currencyCode, decimal value, Guid paypalOrderId, Guid createdBy, DateTime createdOn, Guid modifiedBy, DateTime modifiedOn)
        {
            Id = id;
            Name = name;
            Description = description;
            Quantity = quantity;
            CurrencyCode = currencyCode;
            Value = value;
            PaypalOrderId = paypalOrderId;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            ModifiedOn = modifiedOn;
            CreatedOn = createdOn;
        }

        public static PayPalItem Create(Guid id, string name, string description, int quantity, string currencyCode, decimal value, Guid paypalOrderId, Guid createdBy)
        {
            return new PayPalItem(id, name, description, quantity, currencyCode, value, paypalOrderId, createdBy, DateTime.Now, createdBy, DateTime.Now);
        }
    }
}
