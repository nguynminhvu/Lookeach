using Lookeach.Application.Features.Paypals.Models;

namespace Lookeach.Application.Interfaces.Services
{
    public interface IPayPalService
    {
        Task<string> CreateOrderPayPal(PayPalOrderModel payPalOrderModel);

        // Payed status
        Task<string> CompletedOrder(string token);
    }
}
