using Lookeach.Application.Features.Auths.Models.Requests;
using Lookeach.Application.Interfaces;
using Lookeach.Application.Services;
using Lookeach.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Auths.Commands
{
    public record AddPermissionIntoRoleCommand(AddPermissionModel AddPermissionModel) : IRequest<bool>
    {
    }

    public class AddPermissionIntoRoleCommandHandler : IRequestHandler<AddPermissionIntoRoleCommand, bool>
    {
        private readonly IPermissionService _permissionService;
        private readonly ICleanContext _context;
        private readonly ITokenService _tokenService;

        public AddPermissionIntoRoleCommandHandler(ICleanContext cleanContext, IPermissionService permissionService, ITokenService tokenService)
        {
            _permissionService = permissionService;
            _context = cleanContext;
            _tokenService = tokenService;
        }

        public async Task<bool> Handle(AddPermissionIntoRoleCommand request, CancellationToken cancellationToken)
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

            List<Permission> permissionList = new List<Permission>();
            foreach (var item in actionSetIds.Keys)
            {
                if (permissions.Where(x => x.ResourceId == item && x.RoleId == request.AddPermissionModel.RoleId).ToList().Any() is false)
                {
                    var listAction = actionSetIds[item];
                    if (listAction.Any())
                    {
                        var actionNow = actionSystem.Where(x => listAction.Contains(x.Id)).OrderBy(x => x.Index).ToList();
                        Permission newPermission = new Permission
                        {
                            Id = Guid.NewGuid(),
                            PermissionValue = actionNow.Sum(x => x.Value),
                            RoleId = request.AddPermissionModel.RoleId,
                            ResourceId = item
                        };
                        permissionList.Add(newPermission);
                    }
                }
            }
            await _context.Permissions.AddRangeAsync(permissionList);
            var rs = await _context.SaveChangesAsync(new CancellationToken());

            await _permissionService.InvalidPermissionCache();
            return rs > 0;
        }
    }
}
