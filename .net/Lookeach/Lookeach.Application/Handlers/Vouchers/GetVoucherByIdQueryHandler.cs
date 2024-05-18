using Lookeach.Application.Queries.Vouchers;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Vouchers;
using MediatR;

namespace Lookeach.Application.Handlers.Vouchers
{
    public class GetVoucherByIdQueryHandler : IRequestHandler<GetVoucherByIdQuery, VoucherViewModel>
    {
        private readonly IVoucherService _voucherService;

        public GetVoucherByIdQueryHandler(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }
        public async Task<VoucherViewModel> Handle(GetVoucherByIdQuery request, CancellationToken cancellationToken)
        {
            return await _voucherService.GetVoucherById(request.id);
        }
    }
}
