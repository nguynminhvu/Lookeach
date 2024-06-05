using Lookeach.Application.Features.Auths.Models.Requests;
using Lookeach.Application.Interfaces;
using Lookeach.Application.Services;
using Lookeach.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Auths.Commands
{
    public record AddPermissionIntoRoleCommand(UpdatePermissionActionValue UpdatePermissionActionValue) : IRequest<bool>
    {
    }
    /// <summary>
    /// Add and update permission of permission group(role)
    /// </summary>
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

        /// <summary>
        /// Clear all resources before update
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> Handle(AddPermissionIntoRoleCommand request, CancellationToken cancellationToken)
        {
            if (request.UpdatePermissionActionValue.ResourceValueRecords.Any() is false) throw new ArgumentNullException("Action is required");

            var actionSystem = await _context.Actions.ToListAsync();

            var permissions = await _context.Permissions.Where(x => x.RoleId == request.UpdatePermissionActionValue.RoleId)
                .ToListAsync();

            _context.Permissions.RemoveRange(permissions);
            List<Permission> permissionList = new List<Permission>();
            List<Permission> permissionListUpdate = new List<Permission>();
            foreach (var item in request.UpdatePermissionActionValue.ResourceValueRecords)
            {
                Permission newPermission = new Permission
                {
                    Id = Guid.NewGuid(),
                    PermissionValue = item.PermissionValue,
                    RoleId = request.UpdatePermissionActionValue.RoleId,
                    ResourceId = item.ResourceId
                };
                permissionList.Add(newPermission);
                //if (permissions.Where(x => x.ResourceId == item.ResourceId && x.RoleId == request.UpdatePermissionActionValue.RoleId).ToList().Any() is false)
                //{
                //    Permission newPermission = new Permission
                //    {
                //        Id = Guid.NewGuid(),
                //        PermissionValue = item.PermissionValue,
                //        RoleId = request.UpdatePermissionActionValue.RoleId,
                //        ResourceId = item.ResourceId
                //    };
                //    permissionList.Add(newPermission);
                //}
                //else
                //{
                //    var permissionU = permissions.FirstOrDefault(x => x.RoleId == request.UpdatePermissionActionValue.RoleId);
                //    if (permissionU != null)
                //    {
                //        permissionU.PermissionValue = item.PermissionValue;
                //        permissionListUpdate.Add(permissionU);
                //    }
                //}
            }

            //if (permissionListUpdate.Count > 0)
            //{
            //    _context.Permissions.UpdateRange(permissionListUpdate);
            //}
            await _context.Permissions.AddRangeAsync(permissionList);
            var rs = await _context.SaveChangesAsync(new CancellationToken());
            await _permissionService.InvalidPermissionCache();
            return rs > 0;
        }
    }
}
