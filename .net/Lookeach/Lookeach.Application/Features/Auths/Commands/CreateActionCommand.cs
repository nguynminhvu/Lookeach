using Lookeach.Application.Interfaces;
using Lookeach.Application.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Auths.Commands
{
    public record CreateActionCommand(string Name) : IRequest<bool>
    {
    }
    public class CreateActionCommandHandler : IRequestHandler<CreateActionCommand, bool>
    {
        private readonly IActionService _actionService;
        private readonly IPermissionService _permissionService;
        private readonly ICleanContext _context;

        public CreateActionCommandHandler(ICleanContext cleanContext, IPermissionService permissionService, IActionService actionService)
        {
            _actionService = actionService;
            _permissionService = permissionService;
            _context = cleanContext;
        }

        public async Task<bool> Handle(CreateActionCommand request, CancellationToken cancellationToken)
        {
            var actions = await _context.Actions.ToListAsync();
            if (actions.Any() is false) throw new Exception("No action in system");
            var actionCheck = actions.Where(x => x.Name.ToLower() == request.Name.ToLower()).ToList();
            if (actionCheck.Any()) throw new Exception("Action already exist in system");

            actions = actions.OrderBy(x => x.Index).ToList();
            Lookeach.Core.Entities.Action action = new Core.Entities.Action
            {
                Id = Guid.NewGuid(),
                Index = actions.Last().Index + 1,
                Value = (int)Math.Pow(2, (actions.Last().Index + 1)),
            };

            await _context.Actions.AddAsync(action);
            var rs = await _context.SaveChangesAsync(new CancellationToken());
            await _actionService.InvalidActionCache();
            return rs > 0;
        }
    }
}
