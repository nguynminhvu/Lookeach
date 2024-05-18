using Lookeach.Application.ViewModels.Orders;
using MediatR;

namespace Lookeach.Application.Queries.Orders
{
    public record GetOrdersQuery(DateTime? dateFrom = null, DateTime? dateTo = null, int? pageIndex = null!, int? pageSize = null!):IRequest<List<OrderViewModel>>
    {
    }
}
