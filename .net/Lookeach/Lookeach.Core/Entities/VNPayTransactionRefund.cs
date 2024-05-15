using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities;

public class VNPayTransactionRefund : Audit<DateTime, Guid>
{
    public string ResponseId { get; set; }
    public string TmnCode { get; set; }
    public string TxnRef { get; set; }
    public double Amount { get; set; }
    public string BankCode { get; set; }
    public DateTime PayDate { get; set; }
    public string TransactionNo { get; set; }
    public Guid OrderId { get; set; }
}