using Lookeach.Application.AutoMappers;
using Lookeach.Application.Features.Chats.Models;
using Lookeach.Application.Interfaces;
using Lookeach.Application.Interfaces.Services;
using Lookeach.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Chats.Queries
{
    public record GetChatsQuery(string? receiverName = null!, int? pageIndex = null!, int? pageSize = null!) : IRequest<List<ChatViewModel>>
    {
    }

    public class GetChatsQueryHandler : IRequestHandler<GetChatsQuery, List<ChatViewModel>>
    {
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly ICleanContext _context;

        public GetChatsQueryHandler(ICleanContext cleanContext, IIdentityUser<Guid> identityUser)
        {
            _identityUser = identityUser;
            _context = cleanContext;
        }

        public async Task<List<ChatViewModel>> Handle(GetChatsQuery request, CancellationToken cancellationToken)
        {
            if (_identityUser.UserId == Guid.Empty) throw new UnauthorizedAccessException("Login session expired");

            IQueryable<Chat> queryChat = _context.Chats;
            if (!string.IsNullOrEmpty(request.receiverName))
            {
                var userIds = await _context.Users.Where(x => x.Name.ToLower().Contains(request.receiverName.ToLower())).Select(x => x.Id).ToListAsync();
                var participantChats = await _context.ParticipantChats.Where(x => userIds.Contains(x.ReceiverId)).Select(x => x.ChatId).ToListAsync();
                queryChat = queryChat.Where(x => participantChats.Contains(x.Id));
            }
            queryChat = queryChat.OrderByDescending(x => x.ModifiedOn);
            if (request.pageIndex.HasValue && request.pageSize.HasValue)
            {
                int index = request.pageIndex.Value > 0 ? request.pageIndex.Value - 1 : 0;
                int size = request.pageSize.Value > 0 ? request.pageSize.Value : 10;
                queryChat.Skip(index * size).Take(size);
            }
            queryChat = queryChat.Include(x => x.Messages).ThenInclude(x=>x.User).AsNoTracking().Include(x => x.ChatUsers).ThenInclude(x=>x.User);
            return AutoMapperConfig.Mapper.Map<List<ChatViewModel>>(await queryChat.ToListAsync());
        }
    }
}
