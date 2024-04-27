using Lookeach.Application.Commands.Auths;
using Lookeach.Application.Services;
using MediatR;

namespace Lookeach.Application.Handlers.Auths
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
    {
        private readonly IAuthService _authService;

        public LogoutCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await _authService.Logout();
            return Unit.Value;
        }
    }
}
