using Lookeach.Core.Entities;
using Lookeach.Core.ViewModels.VNPays;

namespace Lookeach.Application.Services
{
    public interface IVNPayService
    {
        Task<string> GetUrlPayment(Order order);
        Task<VNPayViewModel> HandleQuery(string query);
        //Task<List<VNPayTransactionViewModel>> GetVNPayTransactions(VNPayFilter vNPayFilter);
        //Task<VNPayTransactionViewModel> GetVNPayTransactionByTransactionId(Guid id);
        //Task<VNPayViewModel> RefundVNPay(Guid id, Guid userId);
    }
}
