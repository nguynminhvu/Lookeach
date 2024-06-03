using Lookeach.Application.AutoMappers;
using Lookeach.Application.Features.Auths.Models.Reads;
using Lookeach.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Auths.Queries
{
    public class GetAllActionQuery : IRequest<List<ActionViewModel>>
    {
    }
    public class GetAllActionQueryHandler : IRequestHandler<GetAllActionQuery, List<ActionViewModel>>
    {
        private readonly ICleanContext _context;

        public GetAllActionQueryHandler(ICleanContext cleanContext)
        {
            _context = cleanContext;
        }

        public async Task<List<ActionViewModel>> Handle(GetAllActionQuery request, CancellationToken cancellationToken)
        {
            var actions = await _context.Actions.ToListAsync();
            if (actions == null || actions.Count <= 0) throw new Exception("No action in system");
            return AutoMapperConfig.Mapper.Map<List<ActionViewModel>>(actions);
        }
    }
}
