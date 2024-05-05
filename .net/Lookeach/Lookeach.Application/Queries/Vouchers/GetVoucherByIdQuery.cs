using Lookeach.Core.ViewModels.Vouchers;
using MediatR;

namespace Lookeach.Application.Queries.Vouchers
{
    public record GetVoucherByIdQuery(Guid id):IRequest<VoucherViewModel>
    {
    }
}
