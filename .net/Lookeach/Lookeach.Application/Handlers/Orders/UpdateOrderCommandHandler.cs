using Lookeach.Application.Commands.Orders;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Orders;
using MediatR;

namespace Lookeach.Application.Handlers.Orders
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, OrderViewModel>
    {
        private readonly IOrderService _orderService;

        public UpdateOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<OrderViewModel> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var rs = await _orderService.UpdateOrder(request.OrderUpdateModel);
            return rs != null ? rs : throw new Exception("Can't create order");
        }
    }
}
