using Lookeach.Core.ViewModels.Orders;
using MediatR;

namespace Lookeach.Application.Commands.VNPays
{
    public record RefundVNPayCommand(Guid Id) : IRequest<OrderViewModel>
    {
    }
}
