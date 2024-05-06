using Lookeach.Application.Commands.PaymentMethods;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.PaymentMethods;
using MediatR;

namespace Lookeach.Application.Handlers.PaymentMethods
{
    public class UpdatePaymentMethodCommandHandler : IRequestHandler<UpdatePaymentMethodCommand, PaymentMethodViewModel>
    {
        private IPaymentMethodService _paymentMethodService;

        public UpdatePaymentMethodCommandHandler(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        public async Task<PaymentMethodViewModel> Handle(UpdatePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            var rs = await _paymentMethodService.UpdatePaymentMethod(request.PaymentMethodUpdateModel);
            return rs != null ? rs : throw new Exception("Can't update this payment method");
        }
    }
}
