using Lookeach.Application.ViewModels.Orders;
using MediatR;

namespace Lookeach.Application.Commands.Orders
{
    public record CreateOrderCommand(OrderCreateModel OrderCreateModel):IRequest<OrderViewModel>
    {
    }
}
