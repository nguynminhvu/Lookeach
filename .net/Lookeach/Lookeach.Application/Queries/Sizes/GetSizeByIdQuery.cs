using Lookeach.Core.ViewModels.Sizes;
using MediatR;

namespace Lookeach.Application.Queries.Sizes
{
    public record GetSizeByIdQuery(Guid id) : IRequest<SizeViewModel>
    {
    }
}
