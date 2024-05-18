using Lookeach.Application.ViewModels.PaymentMethods;

namespace Lookeach.Application.Services
{
    public interface IPaymentMethodService
    {
        Task<PaymentMethodViewModel> CreatePaymentMethod(string name);
        Task<PaymentMethodViewModel> UpdatePaymentMethod(PaymentMethodUpdateModel paymentMethodUpdateModel);
        Task<PaymentMethodViewModel> GetPaymentMethodById(Guid id);
        Task<List<PaymentMethodViewModel>> GetPaymentMethods(string? name = null!);
    }
}
