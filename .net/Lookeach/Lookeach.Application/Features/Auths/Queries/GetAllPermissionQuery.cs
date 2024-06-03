using Lookeach.Application.AutoMappers;
using Lookeach.Application.Features.Auths.Models.Reads;
using Lookeach.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Auths.Queries
{
    public class GetAllPermissionQuery:IRequest<List<PermissionViewModel>>
    {

    }
    public class GetAllPermissionQueryHandler : IRequestHandler<GetAllPermissionQuery, List<PermissionViewModel>>
    {
        private readonly ICleanContext _context;

        public GetAllPermissionQueryHandler(ICleanContext cleanContext)
        {
            _context=cleanContext;
        }
        public async Task<List<PermissionViewModel>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
        {
            var permissions = await _context.Permissions.Include(x=>x.Resource).Include(x=>x.Role).ToListAsync();
            if (permissions.Any() is false) throw new Exception("No permission in system");
            return AutoMapperConfig.Mapper.Map<List<PermissionViewModel>>(permissions);
        }
    }

}
