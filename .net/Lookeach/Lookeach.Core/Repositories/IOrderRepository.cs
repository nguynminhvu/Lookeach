using Lookeach.Core.Entities;
using Lookeach.Core.Repositories.Base;

namespace Lookeach.Core.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<List<Order>> GetOrders(DateTime? dateFrom = null, DateTime? dateTo = null, int? pageIndex = null!, int? pageSize = null!, string? includes = null!);
        Task<List<Order>> GetOrdersByUserId(Guid userId, DateTime? dateFrom = null, DateTime? dateTo = null, int? pageIndex = null!, int? pageSize = null!, string? includes = null!);
        Task<Order> GetOrder(Guid id);
    }
}
