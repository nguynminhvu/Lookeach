using Lookeach.Application.ViewModels.GenderTypes;
using MediatR;

namespace Lookeach.Application.Commands.GenderTypes
{
    public record UpdateGenderTypeCommand(GenderTypeUpdateModel GenderTypeUpdateModel):IRequest<GenderTypeViewModel>
    {
    }
}
