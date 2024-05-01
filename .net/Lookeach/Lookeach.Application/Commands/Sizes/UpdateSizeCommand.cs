using Lookeach.Core.ViewModels.Sizes;
using MediatR;

namespace Lookeach.Application.Commands.Sizes
{
    public record UpdateSizeCommand(SizeUpdateModel SizeUpdateModel) : IRequest<SizeViewModel>
    {
    }
}
