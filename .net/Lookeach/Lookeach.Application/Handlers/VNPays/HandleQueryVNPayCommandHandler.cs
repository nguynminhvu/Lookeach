using Lookeach.Application.Commands.VNPays;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Orders;
using MediatR;

namespace Lookeach.Application.Handlers.VNPays
{
    public class HandleQueryVNPayCommandHandler : IRequestHandler<HandleQueryVNPayCommand, OrderViewModel>
    {
        private readonly IVNPayService _vnPayService;

        public HandleQueryVNPayCommandHandler(IVNPayService vNPayService)
        {
            _vnPayService = vNPayService;
        }

        public async Task<OrderViewModel> Handle(HandleQueryVNPayCommand request, CancellationToken cancellationToken)
        {
            var rs = await _vnPayService.HandleQuery(request.query);
            return rs != null ? rs : throw new Exception("Can't redirect payment page");
        }
    }
}
