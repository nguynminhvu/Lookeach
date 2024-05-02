using Lookeach.Core.ViewModels.Colors;
using MediatR;

namespace Lookeach.Application.Commands.Colors
{
    public record UpdateColorCommand(ColorUpdateModel ColorUpdateModel):IRequest<ColorViewModel>
    {
    }
}
