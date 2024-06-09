using Lookeach.Application.Interfaces;
using Lookeach.Application.Services;
using MediatR;

namespace Lookeach.Application.Features.Auths.Commands
{
    public record SuperAdminLogoutCommand(string Token) : IRequest<bool>
    {
    }
    public class SuperAdminLogoutCommandHandler : IRequestHandler<SuperAdminLogoutCommand, bool>
    {
        private readonly ICleanContext _context;
        private readonly ITokenService _tokenService;

        public SuperAdminLogoutCommandHandler(ICleanContext cleanContext, ITokenService tokenService)
        {
            _context = cleanContext;
            _tokenService = tokenService;
        }
        public async Task<bool> Handle(SuperAdminLogoutCommand request, CancellationToken cancellationToken)
        {
            return   await _tokenService.DeleteAccessToken(request.Token);
        }
    }
}
