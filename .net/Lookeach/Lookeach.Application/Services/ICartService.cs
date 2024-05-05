using Lookeach.Core.ViewModels.Carts;

namespace Lookeach.Application.Services
{
    public interface ICartService
    {
        Task<CartViewModel> UpdateCart(CartUpdateModel cartUpdateModel);
        Task<CartViewModel> GetCart();
    }
}
