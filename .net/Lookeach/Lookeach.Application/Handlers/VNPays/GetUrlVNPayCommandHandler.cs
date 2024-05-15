using Lookeach.Application.Commands.VNPays;
using Lookeach.Application.Services;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using MediatR;

namespace Lookeach.Application.Handlers.VNPays
{
    public class GetUrlVNPayCommandHandler : IRequestHandler<GetUrlVNPayCommand, string>
    {
        private readonly IUnitOfWork _uow;
        private readonly IOrderRepository _orderRepository;
        private readonly IVNPayService _vnPayService;
        private readonly IOrderService _orderService;

        public GetUrlVNPayCommandHandler(IVNPayService vNPayService, IOrderService orderService, IUnitOfWork unitOfWork)
        {
            _uow= unitOfWork;
            _orderRepository = _uow.OrderRepository;
            _vnPayService = vNPayService;
            _orderService = orderService;
        }
        public async Task<string> Handle(GetUrlVNPayCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrder(request.Id);
            if (order == null!) throw new ArgumentNullException("Not found order");
            var rs = await _vnPayService.GetUrlPayment(order);
            return rs != null ? rs : throw new Exception("Can't redirect payment page");
        }
    }
}
