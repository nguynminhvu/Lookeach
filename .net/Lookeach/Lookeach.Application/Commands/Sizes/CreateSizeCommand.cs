using Lookeach.Core.ViewModels.Sizes;
using MediatR;

namespace Lookeach.Application.Commands.Sizes
{
    public record CreateSizeCommand(string name) : IRequest<SizeViewModel>
    {
    }
}
