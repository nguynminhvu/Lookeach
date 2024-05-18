using Lookeach.Application.Commands.Auths;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Users;
using MediatR;

namespace Lookeach.Application.Handlers.Auths
{
    public class ConfimEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, UserViewModel>
    {
        private readonly IAuthService _authService;

        public ConfimEmailCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<UserViewModel> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            return await _authService.ConfirmEmail(request.code);
        }
    }
}
