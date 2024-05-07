using Lookeach.Application.Queries.Orders;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Orders;
using MediatR;

namespace Lookeach.Application.Handlers.Orders
{
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, List<OrderViewModel>>
    {
        private readonly IOrderService _orderService;

        public GetOrdersByUserIdQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<List<OrderViewModel>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _orderService.GetOrdersByUserId(request.id, request.dateFrom, request.dateTo, request.pageIndex, request.pageSize);
        }
    }
}
