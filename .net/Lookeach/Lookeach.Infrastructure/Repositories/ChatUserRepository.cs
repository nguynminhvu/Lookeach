using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;

namespace Lookeach.Infrastructure.Repositories
{
    public class ChatUserRepository : Repository<ChatUser>, IChatUserRepository
    {
        public ChatUserRepository(LookeachContext appDbContext) : base(appDbContext)
        {
        }
    }
}
