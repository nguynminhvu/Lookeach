using Lookeach.Application.ViewModels.Colors;
using MediatR;

namespace Lookeach.Application.Commands.Colors
{
    public record CreateColorCommand(string name):IRequest<ColorViewModel>
    {
    }
}
