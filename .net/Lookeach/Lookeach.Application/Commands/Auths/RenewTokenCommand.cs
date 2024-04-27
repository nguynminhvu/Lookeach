using MediatR;

namespace Lookeach.Application.Commands.Auths
{
    public record RenewTokenCommand(string token) : IRequest<string>
    {
    }
}
