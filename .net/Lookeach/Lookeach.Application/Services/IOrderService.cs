using Lookeach.Core.ViewModels.Orders;

namespace Lookeach.Application.Services
{
    public interface IOrderService
    {
        Task<OrderViewModel> CreateOrder(OrderCreateModel orderCreateModel);
        Task<OrderViewModel> UpdateOrder(OrderUpdateModel orderUpdateModel);
        Task<OrderViewModel> GetOrderById(Guid id);
        Task<List<OrderViewModel>> GetOrdersByUserId(Guid id, DateTime? dateFrom = null, DateTime? dateTo = null, int? pageIndex = null!, int? pageSize = null!);
        Task<List<OrderViewModel>> GetOrders(DateTime? dateFrom = null, DateTime? dateTo = null, int? pageIndex = null!, int? pageSize = null!);
    }
}
