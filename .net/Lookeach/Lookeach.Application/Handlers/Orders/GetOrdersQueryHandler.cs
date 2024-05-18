using Lookeach.Application.Queries.Orders;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Orders;
using MediatR;

namespace Lookeach.Application.Handlers.Orders
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderViewModel>>
    {
        private readonly IOrderService _orderService;

        public GetOrdersQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<List<OrderViewModel>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _orderService.GetOrders(request.dateFrom, request.dateTo, request.pageIndex, request.pageSize);
        }
    }
}
