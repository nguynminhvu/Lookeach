using Lookeach.Application.ViewModels.Colors;
using MediatR;

namespace Lookeach.Application.Queries.Colors
{
    public record GetColorByIdQuery(Guid id):IRequest<ColorViewModel>
    {
    }
}
