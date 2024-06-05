using Lookeach.Application.Features.Auths.Models.Requests;
using Lookeach.Application.Interfaces;
using Lookeach.Application.Services;
using Lookeach.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Auths.Commands
{
    public record CreateRoleWithPermissionValueCommand(CreateRolePermissionValueModel CreateRolePermissionValueModel):IRequest<bool>
    {
    }
    public class CreateRoleWithPermissionValueCommandHandler : IRequestHandler<CreateRoleWithPermissionValueCommand, bool>
    {
        private readonly IPermissionService _permissionService;
        private readonly ICleanContext _context;

        public CreateRoleWithPermissionValueCommandHandler(ICleanContext cleanContext, IPermissionService permissionService)
        {
            _permissionService = permissionService;
            _context = cleanContext;
        }
        public async Task<bool> Handle(CreateRoleWithPermissionValueCommand request, CancellationToken cancellationToken)
        {
            var roleCheck = await _context.Roles.Where(x => x.Name.ToLower() == request.CreateRolePermissionValueModel.Name.ToLower()).ToListAsync();
            if (roleCheck.Any()) throw new Exception("Role name already exist");
            var newRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = request.CreateRolePermissionValueModel.Name
            };
            var actionSystem = await _context.Actions.ToListAsync();
            List<Permission> permissions = new List<Permission>();
            foreach (var item in request.CreateRolePermissionValueModel.ResourceRecords)
            {
                var resource = await _context.Resources.FirstOrDefaultAsync(x => x.Id == item.ResourceId);
                if (resource == null) throw new ArgumentNullException("Resource invalid");
                int permissionValue = item.PermissionValue;
                permissions.Add(new Permission
                {
                    ResourceId = item.ResourceId,
                    PermissionValue = permissionValue,
                    RoleId = newRole.Id
                });
            }
            newRole.Permissions = permissions;
            if (request.CreateRolePermissionValueModel.UserIds != null && request.CreateRolePermissionValueModel.UserIds.Count > 0)
            {
                var users = await _context.Users.Include(x => x.Roles).Where(x => request.CreateRolePermissionValueModel.UserIds.Contains(x.Id)).ToListAsync();
                newRole.Users = users;
            }
            _context.Roles.Add(newRole);
            var rs = await _context.SaveChangesAsync(new CancellationToken());
            await _permissionService.InvalidPermissionCache();
            return rs > 0;
        }
    }
}
