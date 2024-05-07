using Lookeach.Core.ViewModels.Orders;
using MediatR;

namespace Lookeach.Application.Commands.Orders
{
    public record UpdateOrderCommand(OrderUpdateModel OrderUpdateModel):IRequest<OrderViewModel>
    {
    }
}
