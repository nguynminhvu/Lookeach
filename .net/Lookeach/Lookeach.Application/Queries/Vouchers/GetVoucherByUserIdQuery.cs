using Lookeach.Application.ViewModels.Vouchers;
using MediatR;

namespace Lookeach.Application.Queries.Vouchers
{
    public record GetVoucherByUserIdQuery(Guid id) : IRequest<List<VoucherViewModel>>
    {
    }
}
