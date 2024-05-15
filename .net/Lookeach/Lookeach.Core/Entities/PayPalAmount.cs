using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class PayPalAmount : Audit<DateTime, Guid>
    {
        public string CurrencyCode { get; set; }
        public double Value { get; set; }
        public string ItemTotalCurrencyCode { get; set; }
        public double ItemTotalValue { get; set; }
        public Guid PaypalOrderId { get; set; }
        public PayPalOrder PaypalOrder { get; set; }
    }
}

