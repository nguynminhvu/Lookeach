using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class PayPalItem : Audit<DateTime, Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string CurrencyCode { get; set; }
        public double Value { get; set; }
        public Guid PaypalOrderId { get; set; }

        public PayPalOrder PaypalOrder { get; set; }
    }
}
