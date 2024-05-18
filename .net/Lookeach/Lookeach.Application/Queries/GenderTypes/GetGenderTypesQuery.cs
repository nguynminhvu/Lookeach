using Lookeach.Application.ViewModels.GenderTypes;
using MediatR;

namespace Lookeach.Application.Queries.GenderTypes
{
    public record GetGenderTypesQuery(string? name=null!, int? pageIndex=null!, int? pageSize=null!):IRequest<List<GenderTypeViewModel>>
    {
    }
}
