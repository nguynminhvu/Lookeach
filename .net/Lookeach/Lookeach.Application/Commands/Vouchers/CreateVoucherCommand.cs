using Lookeach.Application.ViewModels.Vouchers;
using MediatR;

namespace Lookeach.Application.Commands.Vouchers
{
    public record CreateVoucherCommand(VoucherCreateModel VoucherCreateModel) : IRequest<VoucherViewModel>
    {
    }
}
