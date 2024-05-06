using Lookeach.Core.ViewModels.PaymentMethods;
using MediatR;

namespace Lookeach.Application.Queries.PaymentMethods
{
    public record GetPaymentMethodByIdQuery(Guid Id):IRequest<PaymentMethodViewModel>
    {
    }
}
