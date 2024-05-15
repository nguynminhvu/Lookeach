using Lookeach.Core.ViewModels.Orders;
using MediatR;

namespace Lookeach.Application.Commands.VNPays
{
    public record HandleQueryVNPayCommand(string query):IRequest<OrderViewModel>
    {
    }
}
