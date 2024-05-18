using Lookeach.Application.Commands.Orders;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Orders;
using MediatR;

namespace Lookeach.Application.Handlers.Orders
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderViewModel>
    {
        private readonly IOrderService _orderService;

        public CreateOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<OrderViewModel> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var rs = await _orderService.CreateOrder(request.OrderCreateModel);
            return rs != null! ? rs : throw new Exception("Can't create Order");
        }
    }
}
