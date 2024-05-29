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
            _permissionService = permissionService;
            _context = cleanContext;
        }

        public async Task<bool> Handle(UpdatePermissionIntoRoleCommand request, CancellationToken cancellationToken)
        {
            if (request.AddPermissionModel.ResourceRecords.Any() is false) throw new ArgumentNullException("Action is required");

            Dictionary<Guid, List<Guid>> actionSetIds = new Dictionary<Guid, List<Guid>>();
            foreach (var item in request.AddPermissionModel.ResourceRecords)
            {
                List<Guid> actionTemps = new List<Guid>();
                foreach (var item1 in item.ActionIds)
                {
                    actionTemps.Add(item1);
                }
                actionSetIds.Add(item.ResourceId, actionTemps);
            }

            var actionSystem = await _context.Actions.ToListAsync();

            var permissions = await _context.Permissions.Where(x => x.RoleId == request.AddPermissionModel.RoleId)
                .ToListAsync();

            if (permissions.Any() is false) throw new Exception("No permission");

            foreach (var item in permissions)
            {
                item.PermissionValue = 0;
                var listAction = actionSetIds[item.ResourceId];
                if (listAction.Any())
                {
                    var actionNow = actionSystem.Where(x => listAction.Contains(x.Id)).OrderBy(x=>x.Index).ToList();
                    item.PermissionValue = actionNow.Sum(x => x.Value);
                }
            }

            _context.Permissions.UpdateRange(permissions);

            var rs = await _context.SaveChangesAsync(new CancellationToken());
            await _permissionService.InvalidPermissionCache();
            return rs > 0;
        }
    }
}
