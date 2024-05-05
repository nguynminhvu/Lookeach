using Lookeach.Application.Commands.Vouchers;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Vouchers;
using MediatR;

namespace Lookeach.Application.Handlers.Vouchers
{
    public class CreateVoucherCommandHandler : IRequestHandler<CreateVoucherCommand, VoucherViewModel>
    {
        private readonly IVoucherService _voucherService;

        public CreateVoucherCommandHandler(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }
        public async Task<VoucherViewModel> Handle(CreateVoucherCommand request, CancellationToken cancellationToken)
        {
            var rs = await _voucherService.CreateVoucher(request.VoucherCreateModel);
            return rs != null ? rs : throw new Exception("Can't create voucher");
        }
    }
}
