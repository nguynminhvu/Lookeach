using Lookeach.Application.Commands.VNPays;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Orders;
using MediatR;

namespace Lookeach.Application.Handlers.VNPays
{
    public class RefundVNPayCommandHandler : IRequestHandler<RefundVNPayCommand, OrderViewModel>
    {
        private readonly IVNPayService _vnPayService;

        public RefundVNPayCommandHandler(IVNPayService vNPayService)
        {
            _vnPayService = vNPayService;
        }

        public async Task<OrderViewModel> Handle(RefundVNPayCommand request, CancellationToken cancellationToken)
        {
            var rs = await _vnPayService.RefundVNPay(request.Id);
            return rs != null ? rs : throw new Exception("Can't refund this order transaction");
        }
    }
}
