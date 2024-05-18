using Lookeach.Application.Commands.PaymentMethods;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.PaymentMethods;
using MediatR;

namespace Lookeach.Application.Handlers.PaymentMethods
{
    public class CreatePaymentMethodCommandHandler : IRequestHandler<CreatePaymentMethodCommand, PaymentMethodViewModel>
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public CreatePaymentMethodCommandHandler(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }
        public async Task<PaymentMethodViewModel> Handle(CreatePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            var rs = await _paymentMethodService.CreatePaymentMethod(request.Name);
            return rs != null ? rs : throw new Exception("Can't create new Payment Method");
        }
    }
}
