using Lookeach.Application.Features.Auths.Models.Requests;
using Lookeach.Application.Interfaces;
using Lookeach.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Auths.Commands
{
    public record UpdatePermissionIntoRoleActionValueCommand(UpdatePermissionActionValue UpdatePermissionActionValue) : IRequest<bool>
    {
    }

    public class UpdatePermissionIntoRoleActionValueCommandHandler : IRequestHandler<UpdatePermissionIntoRoleActionValueCommand, bool>
    {
        private readonly ICleanContext _context;

        public UpdatePermissionIntoRoleActionValueCommandHandler(ICleanContext cleanContext)
        {
            _context = cleanContext;
        }

        public async Task<bool> Handle(UpdatePermissionIntoRoleActionValueCommand request, CancellationToken cancellationToken)
        {
            var permissionRoles = await _context.Permissions.Where(x => x.RoleId == request.UpdatePermissionActionValue.RoleId).ToListAsync();
            List<Permission> permissionList = new List<Permission>();
            foreach (var item in request.UpdatePermissionActionValue.ResourceValueRecords)
            {
                var permissionRole = permissionRoles.Where(x => x.ResourceId == item.ResourceId).FirstOrDefault()!;
                if (permissionRole != null)
                {
                    permissionRole.PermissionValue = item.PermissionValue;
                    permissionList.Add(permissionRole);
                }
            }
            _context.Permissions.UpdateRange(permissionList);
            var rs = await _context.SaveChangesAsync(new CancellationToken());
            return rs > 0;
        }
    }
}
