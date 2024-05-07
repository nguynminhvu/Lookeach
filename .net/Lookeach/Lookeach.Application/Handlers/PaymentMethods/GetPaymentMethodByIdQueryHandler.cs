using Lookeach.Application.Queries.PaymentMethods;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.PaymentMethods;
using MediatR;

namespace Lookeach.Application.Handlers.PaymentMethods
{
    public class GetPaymentMethodByIdQueryHandler : IRequestHandler<GetPaymentMethodByIdQuery, PaymentMethodViewModel>
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public GetPaymentMethodByIdQueryHandler(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }
        public async Task<PaymentMethodViewModel> Handle(GetPaymentMethodByIdQuery request, CancellationToken cancellationToken)
        {
            return await _paymentMethodService.GetPaymentMethodById(request.Id);
        }
    }
}
