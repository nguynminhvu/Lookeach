using Lookeach.Application.Queries.Vouchers;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Vouchers;
using MediatR;

namespace Lookeach.Application.Handlers.Vouchers
{
    public class GetVoucherSystemQueryHandler : IRequestHandler<GetVoucherSystemQuery, List<VoucherViewModel>>
    {
        private readonly IVoucherService _voucherService;

        public GetVoucherSystemQueryHandler(IVoucherService voucherService)
        {
            _voucherService= voucherService;
        }
        public async Task<List<VoucherViewModel>> Handle(GetVoucherSystemQuery request, CancellationToken cancellationToken)
        {
            return await _voucherService.GetVouchersSystem();
        }
    }
}
