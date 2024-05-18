using Lookeach.Application.Commands.Vouchers;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Vouchers;
using MediatR;

namespace Lookeach.Application.Handlers.Vouchers
{
    public class UpdateVoucherCommandHandler : IRequestHandler<UpdateVoucherCommand, VoucherViewModel>
    {
        private readonly IVoucherService _voucherService;

        public UpdateVoucherCommandHandler(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }
        public async Task<VoucherViewModel> Handle(UpdateVoucherCommand request, CancellationToken cancellationToken)
        {
            var rs = await _voucherService.UpdateVoucher(request.VoucherUpdateModel);
            return rs != null! ? rs : throw new Exception("Can't update this voucher");
        }
    }
}
