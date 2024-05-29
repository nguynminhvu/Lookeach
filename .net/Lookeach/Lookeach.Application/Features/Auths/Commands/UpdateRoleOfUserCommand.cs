using Lookeach.Application.Features.Auths.Models.Requests;
using Lookeach.Application.Interfaces;
using Lookeach.Application.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Auths.Commands
{
    public record UpdateRoleOfUserCommand(UpdateUserRoleModel UpdateUserRoleModel) : IRequest<bool>
    {
    }
    public class UpdateRoleOfUserCommandHandler : IRequestHandler<UpdateRoleOfUserCommand, bool>
    {
        private readonly IPermissionService _permissionService;
        private readonly ICleanContext _context;

        public UpdateRoleOfUserCommandHandler(ICleanContext cleanContext, IPermissionService permissionService)
        {
            _permissionService= permissionService;
            _context = cleanContext;
        }
        public async Task<bool> Handle(UpdateRoleOfUserCommand request, CancellationToken cancellationToken)
        {
            var roleCheck = await _context.Roles.FirstOrDefaultAsync(x => x.Id == request.UpdateUserRoleModel.RoleId);
            if (roleCheck == null) throw new ArgumentNullException("Not found role");

            var userCheck = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UpdateUserRoleModel.UserId);
            if (userCheck == null) throw new ArgumentNullException("Not found user");

            userCheck.RoleId = roleCheck.Id;
            _context.Users.Update(userCheck);
            var rs = await _context.SaveChangesAsync(new CancellationToken());
            await _permissionService.InvalidPermissionCache();
            return rs > 0;
        }
    }
}
