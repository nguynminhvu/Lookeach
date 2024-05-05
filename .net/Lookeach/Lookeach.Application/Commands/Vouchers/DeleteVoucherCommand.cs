using MediatR;

namespace Lookeach.Application.Commands.Vouchers
{
    public record DeleteVoucherCommand(Guid Id) : IRequest<bool>
    {
    }
}
