using Lookeach.Application.Queries.Orders;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Orders;
using MediatR;

namespace Lookeach.Application.Handlers.Orders
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderViewModel>
    {
        private readonly IOrderService _orderService;

        public GetOrderByIdQueryHandler(IOrderService orderService)
        {
            _orderService=orderService;
        }
        public async Task<OrderViewModel> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            return await _orderService.GetOrderById(request.Id);
        }
    }
}
