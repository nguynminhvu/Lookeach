using Lookeach.Application.ViewModels.Vouchers;
using MediatR;

namespace Lookeach.Application.Commands.Vouchers
{
    public record UpdateVoucherCommand(VoucherUpdateModel VoucherUpdateModel):IRequest<VoucherViewModel>
    {
    }
}
