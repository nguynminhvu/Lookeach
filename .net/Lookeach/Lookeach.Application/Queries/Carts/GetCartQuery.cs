using Lookeach.Application.ViewModels.Carts;
using MediatR;

namespace Lookeach.Application.Queries.Carts
{
    public class GetCartQuery:IRequest<CartViewModel>
    {
    }
}
