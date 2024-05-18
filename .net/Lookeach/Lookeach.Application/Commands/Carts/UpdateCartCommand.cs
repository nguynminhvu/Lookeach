using Lookeach.Application.ViewModels.Carts;
using MediatR;

namespace Lookeach.Application.Commands.Carts
{
    public record UpdateCartCommand(CartUpdateModel CartUpdateModel) : IRequest<CartViewModel>
    {
    }
}
