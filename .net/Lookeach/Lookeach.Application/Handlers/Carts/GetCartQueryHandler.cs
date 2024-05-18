using Lookeach.Application.Queries.Carts;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Carts;
using MediatR;

namespace Lookeach.Application.Handlers.Carts
{
    public class GetCartQueryHandler : IRequestHandler<GetCartQuery, CartViewModel>
    {
        private readonly ICartService _cartService;

        public GetCartQueryHandler(ICartService cartService)
        {
            _cartService=cartService;
        }
        public async Task<CartViewModel> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            return await _cartService.GetCart();
        }
    }
}
