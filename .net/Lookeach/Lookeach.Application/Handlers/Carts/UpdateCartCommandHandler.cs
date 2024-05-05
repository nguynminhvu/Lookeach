using Lookeach.Application.Commands.Carts;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Carts;
using MediatR;

namespace Lookeach.Application.Handlers.Carts
{
    public class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, CartViewModel>
    {
        private readonly ICartService _cartService;

        public UpdateCartCommandHandler(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<CartViewModel> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            return await _cartService.UpdateCart(request.CartUpdateModel);
        }
    }
}
