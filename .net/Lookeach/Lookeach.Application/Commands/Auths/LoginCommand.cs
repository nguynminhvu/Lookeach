using Lookeach.Core.DTOs;
using MediatR;

namespace Lookeach.Application.Commands.Auths
{
    public record LoginCommand(LoginDTO LoginDTO) : IRequest<string>
    {
    }
}
