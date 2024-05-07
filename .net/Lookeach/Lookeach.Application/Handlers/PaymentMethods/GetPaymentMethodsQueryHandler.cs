using Lookeach.Application.Queries.PaymentMethods;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.PaymentMethods;
using MediatR;

namespace Lookeach.Application.Handlers.PaymentMethods
{
    public class GetPaymentMethodsQueryHandler : IRequestHandler<GetPaymentMethodsQuery, List<PaymentMethodViewModel>>
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public GetPaymentMethodsQueryHandler(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        public async Task<List<PaymentMethodViewModel>> Handle(GetPaymentMethodsQuery request, CancellationToken cancellationToken)
        {
            return await _paymentMethodService.GetPaymentMethods(request.Name);
        }
    }
}
