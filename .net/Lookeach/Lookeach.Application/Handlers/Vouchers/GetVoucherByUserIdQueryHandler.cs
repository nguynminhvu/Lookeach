using Lookeach.Application.Queries.Vouchers;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Vouchers;
using MediatR;

namespace Lookeach.Application.Handlers.Vouchers
{
    public class GetVoucherByUserIdQueryHandler : IRequestHandler<GetVoucherByUserIdQuery, List<VoucherViewModel>>
    {
        private readonly IVoucherService _voucherService;

        public GetVoucherByUserIdQueryHandler(IVoucherService voucherService)
        {
            _voucherService= voucherService;
        }
        public async Task<List<VoucherViewModel>> Handle(GetVoucherByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _voucherService.GetVoucherByUserId(request.id);
        }
    }
}
