using Lookeach.Core.ViewModels.Colors;
using MediatR;

namespace Lookeach.Application.Queries.Colors
{
    public record GetColorsQuery(string? name = null!, int? pageIndex = null!, int? pageSize = null!) : IRequest<List<ColorViewModel>>
    {
    }
}
