using Lookeach.Application.DTOs;
using Lookeach.Application.Interfaces;
using Lookeach.Application.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Auths.Commands
{
    public record SuperAdminLoginCommand(LoginDTO LoginDTO) : IRequest<string>
    {
    }
    public class SuperAdminLoginCommandHandler : IRequestHandler<SuperAdminLoginCommand, string>
    {
        private readonly IValidateService _validationService;
        private readonly ITokenService _tokenService;
        private readonly ICleanContext _context;

        public SuperAdminLoginCommandHandler(ICleanContext cleanContext, ITokenService tokenService, IValidateService validateService)
        {
            _validationService=validateService;
            _tokenService = tokenService;
            _context = cleanContext;
        }

        public async Task<string> Handle(SuperAdminLoginCommand request, CancellationToken cancellationToken)
        {
            var sAdmin = await _context.SuperAdmins.FirstOrDefaultAsync(x => x.Username == request.LoginDTO.Username && x.Password == _validationService.HashPassword(request.LoginDTO.Password));
            if (sAdmin == null) throw new UnauthorizedAccessException("Wrong username or password");
            return await _tokenService.GenerateToken(sAdmin);
        }
      
    }

}
