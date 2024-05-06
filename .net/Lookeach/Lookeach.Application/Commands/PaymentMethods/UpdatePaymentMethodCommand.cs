using Lookeach.Core.ViewModels.PaymentMethods;
using MediatR;

namespace Lookeach.Application.Commands.PaymentMethods
{
    public record UpdatePaymentMethodCommand(PaymentMethodUpdateModel PaymentMethodUpdateModel) : IRequest<PaymentMethodViewModel>
    {
    }
}
