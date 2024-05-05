using Lookeach.Core.ViewModels.Vouchers;
using MediatR;

namespace Lookeach.Application.Commands.Vouchers
{
    public record UpdateVoucherCommand(VoucherUpdateModel VoucherUpdateModel):IRequest<VoucherViewModel>
    {
    }
}
