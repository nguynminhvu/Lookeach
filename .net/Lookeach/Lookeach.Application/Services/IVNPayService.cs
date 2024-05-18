using Lookeach.Application.ViewModels.Orders;
using Lookeach.Core.Entities;

namespace Lookeach.Application.Services
{
    public interface IVNPayService
    {
        Task<string> GetUrlPayment(Order order);
        Task<OrderViewModel> HandleQuery(string query);
        Task<OrderViewModel> RefundVNPay(Guid orderId);
        Task<List<VNPayTransaction>> GetVNPayTransactions(Guid? id = null!, DateTime? dateFrom = null!, DateTime? dateTo = null!, int? pageIndex=null!, int? pageSize=null!);
    }
}
