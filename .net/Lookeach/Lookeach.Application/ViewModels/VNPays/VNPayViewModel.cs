using Lookeach.Core.Common.Base;

namespace Lookeach.Application.ViewModels.VNPays
{
    public class VNPayViewModel : Audit<DateTime, Guid>
    {
        public double Amount { get; set; }
        public string BankCode { get; set; }
        public string BankTranNo { get; set; }
        public string CardType { get; set; }
        public DateTime PayDate { get; set; }
        public string TmnCode { get; set; }
        public string TransactionNo { get; set; }
        public Guid OrderId { get; set; }
    }
}
