using Lookeach.Application.Features.Auths.Models.Requests;
using Lookeach.Application.Features.Paypals.Models;
using Lookeach.Application.Interfaces;
using Lookeach.Application.Services;
using Lookeach.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Auths.Commands
{
    public record UpdateRoleOfUserCommand(UpdateUserRoleModel UpdateUserRoleModel) : IRequest<bool>
    {
    }
    public class UpdateRoleOfUserCommandHandler : IRequestHandler<UpdateRoleOfUserCommand, bool>
    {
        private readonly ITokenService _tokenService;
        private readonly IPermissionService _permissionService;
        private readonly ICleanContext _context;

        public UpdateRoleOfUserCommandHandler(ICleanContext cleanContext, IPermissionService permissionService, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _permissionService = permissionService;
            _context = cleanContext;
        }

        public async Task<bool> Handle(UpdateRoleOfUserCommand request, CancellationToken cancellationToken)
        {
            var roleCheck = new List<Role>();
            if (request.UpdateUserRoleModel.RoleIds != null && request.UpdateUserRoleModel.RoleIds.Count > 0)
            {
                roleCheck = await _context.Roles.Where(x => request.UpdateUserRoleModel.RoleIds.Contains(x.Id)).ToListAsync();
                if (roleCheck.Any() is false) throw new ArgumentNullException("Not found role");
            }

            var userCheck = await _context.Users.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Id == request.UpdateUserRoleModel.UserId);
            if (userCheck == null) throw new ArgumentNullException("Not found user");

            if (request.UpdateUserRoleModel.RoleIds == null || request.UpdateUserRoleModel.RoleIds.Count <= 0)
            {
                if (userCheck.Roles != null)
                {
                    userCheck.Roles.Clear();
                }
            }
            else
            {
                userCheck.Roles = roleCheck;
            }
            _context.Users.Update(userCheck);
            var rs = await _context.SaveChangesAsync(new CancellationToken());

     
            await _tokenService.InvalidUserRoleCache(request.UpdateUserRoleModel.UserId);
            await _permissionService.InvalidPermissionCache();
            return rs > 0;
        }
    }
}
