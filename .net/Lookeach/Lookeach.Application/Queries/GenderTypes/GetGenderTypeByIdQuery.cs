using Lookeach.Core.ViewModels.GenderTypes;
using MediatR;

namespace Lookeach.Application.Queries.GenderTypes
{
    public record GetGenderTypeByIdQuery(Guid id):IRequest<GenderTypeViewModel>
    {
    }
}
