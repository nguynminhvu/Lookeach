using Lookeach.Application.Commands.Auths;
using Lookeach.Application.Services;
using MediatR;

namespace Lookeach.Application.Handlers.Auths
{
    public class RenewTokenCommandHandler : IRequestHandler<RenewTokenCommand, string>
    {
        private readonly IAuthService _authService;

        public RenewTokenCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<string> Handle(RenewTokenCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RenewToken(request.token);
        }
    }
}
