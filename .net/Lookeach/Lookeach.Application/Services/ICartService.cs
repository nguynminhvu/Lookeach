using Lookeach.Application.ViewModels.Carts;

namespace Lookeach.Application.Services
{
    public interface ICartService
    {
        Task<CartViewModel> UpdateCart(CartUpdateModel cartUpdateModel);
        Task<CartViewModel> GetCart();
    }
}
