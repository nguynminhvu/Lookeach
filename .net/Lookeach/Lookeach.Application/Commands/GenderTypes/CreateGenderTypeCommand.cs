using Lookeach.Application.ViewModels.GenderTypes;
using MediatR;

namespace Lookeach.Application.Commands.GenderTypes
{
    public record CreateGenderTypeCommand(string name):IRequest<GenderTypeViewModel>
    {
    }
}
