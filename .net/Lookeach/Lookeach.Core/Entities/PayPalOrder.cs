using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class PayPalOrder : Audit<DateTime, Guid>
    {
        public string Token { get; private set; }
        public string? CaptureId { get; private set; }
        public string Intent { get; set; }
        public ICollection<PayPalItem> PayPalItems { get; private set; } = new List<PayPalItem>();
        public PayPalAmount PayPalAmounts { get; private set; } 
        public string Status { get; private set; }
        public string PayeeEmailAddress { get; private set; }
        public string MerchantId { get; private set; }
        public decimal ExchangeCurrency { get; private set; }
        public Guid OrderId { get; private set; }
        public Order Order { get; set; }

        private PayPalOrder()
        {
            
        }
        protected PayPalOrder(Guid id, string token, string? captureId, string intent, string status, string payeeEmail, string merchantId, decimal exchangCurrency, Guid orderId, Guid createdBy, DateTime createdOn, Guid modifiedBy, DateTime modifiedOn)
        {
            Id = id;
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Can't create PayPal transaction, Token null!!");
            }
            Token = token;
            if (captureId!=null!)
            {
                CaptureId = captureId;
            }
            Intent = intent;
            Status = status;
            PayeeEmailAddress = payeeEmail;
            MerchantId = merchantId;
            PayeeEmailAddress = payeeEmail;
            MerchantId = merchantId;
            ExchangeCurrency = exchangCurrency;
            OrderId = orderId;
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            ModifiedBy = modifiedBy;
            ModifiedOn = modifiedOn;
        }

        public static PayPalOrder Create(Guid id, string token, string intent, string status, string payeeEmail, string merchantId, decimal exchangCurrency, Guid orderId, Guid createdBy, string? captureId = null!)
        {
            return new PayPalOrder(id, token, captureId, intent, status, payeeEmail, merchantId, exchangCurrency, orderId, createdBy, DateTime.Now, createdBy, DateTime.Now);
        }

        public void UpdatePayPaylOrderCompleted(string capture, string status, Guid modifiedBy)
        {
            CaptureId = capture;
            Status = status;
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTime.Now;
        }
    }
}
