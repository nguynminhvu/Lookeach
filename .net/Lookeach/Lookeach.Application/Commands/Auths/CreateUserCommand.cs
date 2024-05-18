using Lookeach.Application.ViewModels.Users;
using MediatR;

namespace Lookeach.Application.Commands.Auths
{
    public record CreateUserCommand(UserCreateModel UserCreateModel, Guid id) : IRequest<UserViewModel>
    {
    }
}
