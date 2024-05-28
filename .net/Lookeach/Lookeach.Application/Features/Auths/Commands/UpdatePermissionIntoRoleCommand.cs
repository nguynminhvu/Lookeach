using Lookeach.Application.Features.Auths.Models.Requests;
using Lookeach.Application.Interfaces;
using Lookeach.Application.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Auths.Commands
{
    public record UpdatePermissionIntoRoleCommand(AddPermissionModel AddPermissionModel) : IRequest<bool>
    {
    }

    public class UpdatePermissionIntoRoleCommandHandler : IRequestHandler<UpdatePermissionIntoRoleCommand, bool>
    {
        private readonly IPermissionService _permissionService;
        private readonly ICleanContext _context;

        public UpdatePermissionIntoRoleCommandHandler(ICleanContext cleanContext, IPermissionService permissionService)
        {
            _permissionService= permissionService;
            _context = cleanContext;
        }

        public async Task<bool> Handle(UpdatePermissionIntoRoleCommand request, CancellationToken cancellationToken)
        {
            if (request.AddPermissionModel.ActionIds.Any() is false) throw new ArgumentNullException("Action is required");

            var actionSystem = await _context.Actions.Where(x => request.AddPermissionModel.ActionIds.Contains(x.Id)).ToListAsync();

            actionSystem = actionSystem.OrderBy(x => x.Index).ToList();

            var permissions = await _context.Permissions.Where(x => x.RoleId == request.AddPermissionModel.RoleId && x.ResourceId == request.AddPermissionModel.ResourceId).FirstOrDefaultAsync();

            if (permissions == null) throw new Exception("No permission");

            permissions.PermissionValue = 0;

            permissions.PermissionValue = actionSystem.Sum(x => x.Value);

            _context.Permissions.Update(permissions);

            var rs = await _context.SaveChangesAsync(new CancellationToken());
            await _permissionService.InvalidPermissionCache();
            return rs > 0;
        }
    }
}
