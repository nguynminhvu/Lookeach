using Lookeach.Application.Commands.Vouchers;
using Lookeach.Application.Services;
using MediatR;

namespace Lookeach.Application.Handlers.Vouchers
{
    public class DeleteVoucherCommandHandler : IRequestHandler<DeleteVoucherCommand, bool>
    {
        private readonly IVoucherService _voucherService;

        public DeleteVoucherCommandHandler(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }
        public async Task<bool> Handle(DeleteVoucherCommand request, CancellationToken cancellationToken)
        {
            var rs = await _voucherService.DeleteVoucher(request.Id);
            return rs;
        }
    }
}
