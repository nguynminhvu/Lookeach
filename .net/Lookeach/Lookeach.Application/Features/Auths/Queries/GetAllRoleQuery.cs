using Lookeach.Application.AutoMappers;
using Lookeach.Application.Features.Auths.Models.Reads;
using Lookeach.Application.Interfaces;
using Lookeach.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Auths.Queries
{
    public record GetAllRoleQuery(string? Name = null!) : IRequest<List<RoleViewModel>>
    {
    }
    public class GetAllRoleQueryHandler : IRequestHandler<GetAllRoleQuery, List<RoleViewModel>>
    {
        private readonly ICleanContext _context;

        public GetAllRoleQueryHandler(ICleanContext cleanContext)
        {
            _context = cleanContext;
        }
        public async Task<List<RoleViewModel>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Role> query = _context.Roles;

            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(request.Name.ToLower()));
            }
            return AutoMapperConfig.Mapper.Map<List<RoleViewModel>>(await query.ToListAsync());
        }
    }
}
