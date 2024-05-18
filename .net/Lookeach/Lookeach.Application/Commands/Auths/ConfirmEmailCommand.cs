using Lookeach.Application.ViewModels.Users;
using MediatR;

namespace Lookeach.Application.Commands.Auths
{
    public record ConfirmEmailCommand(string code) : IRequest<UserViewModel>
    {
    }
}
