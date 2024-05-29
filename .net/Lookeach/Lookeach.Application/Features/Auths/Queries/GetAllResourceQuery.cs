using Lookeach.Application.AutoMappers;
using Lookeach.Application.Features.Auths.Models.Reads;
using Lookeach.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Auths.Queries
{
    public class GetAllResourceQuery:IRequest<List<ResourceViewModel>>
    {

    }
    public class GetAllResourceQueryHandler : IRequestHandler<GetAllResourceQuery, List<ResourceViewModel>>
    {
        private readonly ICleanContext _context;

        public GetAllResourceQueryHandler(ICleanContext cleanContext)
        {
            _context=cleanContext;
        }
        public async Task<List<ResourceViewModel>> Handle(GetAllResourceQuery request, CancellationToken cancellationToken)
        {
            var resources = await _context.Resources.ToListAsync();
            if (resources.Any() is false) throw new Exception("No resource in system");
            return AutoMapperConfig.Mapper.Map<List<ResourceViewModel>>(resources);
        }
    }
}
