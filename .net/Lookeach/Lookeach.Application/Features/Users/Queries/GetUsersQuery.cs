using Lookeach.Application.AutoMappers;
using Lookeach.Application.Interfaces;
using Lookeach.Application.ViewModels.Users;
using Lookeach.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Users.Queries
{
    public record GetUsersQuery(string? Name = null!, string? Username = null!, string? Email = null!, int? PageIndex = null!, int? PageSize = null!) : IRequest<List<UserViewModel>>
    {

    }
    public class GetUsersQueryHandlers : IRequestHandler<GetUsersQuery, List<UserViewModel>>
    {
        private readonly ICleanContext _context;

        public GetUsersQueryHandlers(ICleanContext cleanContext)
        {
            _context = cleanContext;
        }
        public async Task<List<UserViewModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            IQueryable<User> query = _context.Users;
            if (!string.IsNullOrEmpty(request.Username))
            {
                query = query.Where(x => x.Username.ToLower().Contains(request.Username.ToLower()));
            }
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(request.Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(request.Email))
            {
                query = query.Where(x => x.Email.ToLower().Contains(request.Email.ToLower()));
            }
            if (request.PageIndex.HasValue && request.PageSize.HasValue)
            {
                int index = request.PageIndex.Value > 0 ? request.PageIndex.Value - 1 : 0;
                int size = request.PageSize.Value > 0 ? request.PageSize.Value : 10;
                query = query.Skip(index * size).Take(size);
            }
            query = query.Include(x => x.Roles);

            return AutoMapperConfig.Mapper.Map<List<UserViewModel>>(await query.ToListAsync());
        }
    }
}
