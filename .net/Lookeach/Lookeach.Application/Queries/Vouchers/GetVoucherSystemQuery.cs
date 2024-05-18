using Lookeach.Application.ViewModels.Vouchers;
using MediatR;

namespace Lookeach.Application.Queries.Vouchers
{
    public record GetVoucherSystemQuery:IRequest<List<VoucherViewModel>>
    {
    }
}
