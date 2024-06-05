using Lookeach.Application.Features.Auths.Models.Requests;
using Lookeach.Application.Interfaces;
using Lookeach.Application.Services;
using Lookeach.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Auths.Commands
{
    public record CreateRoleCommand(CreateRoleModel CreateRoleModel) : IRequest<bool>
    {
    }
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, bool>
    {
        private readonly IPermissionService _permissionService;
        private readonly ICleanContext _context;

        public CreateRoleCommandHandler(ICleanContext cleanContext, IPermissionService permissionService)
        {
            _permissionService = permissionService;
            _context = cleanContext;
        }
        public async Task<bool> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var roleCheck = await _context.Roles.Where(x => x.Name.ToLower() == request.CreateRoleModel.Name.ToLower()).ToListAsync();
            if (roleCheck.Any()) throw new Exception("Role name already exist");
            var newRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = request.CreateRoleModel.Name
            };
            var actionSystem = await _context.Actions.ToListAsync();
            List<Permission> permissions = new List<Permission>();
            foreach (var item in request.CreateRoleModel.ResourceRecords)
            {
                var resource = await _context.Resources.FirstOrDefaultAsync(x => x.Id == item.ResourceId);
                if (resource == null) throw new ArgumentNullException("Resource invalid");
                var permissionActual = actionSystem.Where(x => item.ActionIds.Contains(x.Id));
                int permissionValue = permissionActual.Sum(x => x.Value);
                permissions.Add(new Permission
                {
                    ResourceId = item.ResourceId,
                    PermissionValue = permissionValue,
                    RoleId = newRole.Id
                });
            }
            newRole.Permissions = permissions;
            if (request.CreateRoleModel.UserIds != null && request.CreateRoleModel.UserIds.Count > 0)
            {
                var users = await _context.Users.Include(x => x.Roles).Where(x => request.CreateRoleModel.UserIds.Contains(x.Id)).ToListAsync();
                newRole.Users = users;
            }
            _context.Roles.Add(newRole);
            var rs = await _context.SaveChangesAsync(new CancellationToken());
            await _permissionService.InvalidPermissionCache();
            return rs > 0;
        }
    }
}
