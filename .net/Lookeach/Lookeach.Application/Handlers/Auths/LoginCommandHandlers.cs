using Lookeach.Application.Commands.Auths;
using Lookeach.Application.Services;
using MediatR;

namespace Lookeach.Application.Handlers.Auths
{
    public class LoginCommandHandlers : IRequestHandler<LoginCommand, string>
    {
        private readonly IAuthService _authService;

        public LoginCommandHandlers(IAuthService authService)
        {
            _authService=authService;
        }
        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _authService.Login(request.LoginDTO);
        }
    }
}
