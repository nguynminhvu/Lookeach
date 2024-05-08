using Lookeach.Application.DTOs;
using Lookeach.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookeach.Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        public Task<PermissionDTO> GetPermission(Guid roleId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidPermission(PermissionDTO permissionDTO, string resourceAccess)
        {
            throw new NotImplementedException();
        }
    }
}
