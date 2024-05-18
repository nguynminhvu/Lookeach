using Lookeach.Application.ViewModels.Orders;
using MediatR;

namespace Lookeach.Application.Queries.Orders
{
    public record GetOrderByIdQuery(Guid Id):IRequest<OrderViewModel>
    {
    }
}
