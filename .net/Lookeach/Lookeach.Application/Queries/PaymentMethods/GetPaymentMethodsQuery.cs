using Lookeach.Application.ViewModels.PaymentMethods;
using MediatR;

namespace Lookeach.Application.Queries.PaymentMethods
{
    public record GetPaymentMethodsQuery(string Name) : IRequest<List<PaymentMethodViewModel>>
    {
    }
}
