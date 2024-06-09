using Lookeach.Application.DTOs;
using Lookeach.Application.Interfaces;
using Lookeach.Application.Services;
using Lookeach.Core.Entities;
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
            _validationService = validateService;
            _tokenService = tokenService;
            _context = cleanContext;
        }

        public async Task<string> Handle(SuperAdminLoginCommand request, CancellationToken cancellationToken)
        {
            var sAdmin = await _context.SuperAdmins.FirstOrDefaultAsync(x => x.Username == request.LoginDTO.Username && x.Password == _validationService.HashPassword(request.LoginDTO.Password));
            if (sAdmin == null) throw new UnauthorizedAccessException("Wrong username or password");
            var tk = await _context.AccessTokens.Where(x => x.UserId == sAdmin.Id).ToListAsync();
            if (tk.Any())
            {
                foreach (var item in tk)
                {
                    _context.AccessTokens.Remove(item);
                    await _tokenService.DeleteAccessToken(item.Token);
                }
                await _context.SaveChangesAsync(new CancellationToken());
                throw new UnauthorizedAccessException("Login session expired, another device online try again");
            }
            var tks = await _tokenService.GenerateToken(sAdmin);
            await _tokenService.SaveUserRoleAdminCache(sAdmin.Id);
            await _tokenService.SaveAccessToken(tks, sAdmin.Id);
            return tks;
        }
    }
}
