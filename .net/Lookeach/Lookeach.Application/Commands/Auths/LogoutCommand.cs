using MediatR;

namespace Lookeach.Application.Commands.Auths
{
    public record LogoutCommand : IRequest<Unit>
    {
    }
}
