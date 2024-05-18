using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly LookeachContext _context;

        public UserRepository(LookeachContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }


        public async Task<User> GetUserByEmail(string email)
            => (await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == email && x.IsActived))!;

        public async Task<User> GetUserByEmailAndPassword(string email, string password, string? include = null!)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(email));
            IQueryable<User> query = _context.Users;
            if (!string.IsNullOrEmpty(include))
            {
                foreach (var item in include.Split(","))
                {
                    query = query.Include(item);
                }
            }
            return (await query.FirstOrDefaultAsync(x => x.Email == email && x.Password == password && x.IsActived))!;
        }

        public async Task<User> GetUserById(Guid id)
             => (await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id && x.IsActived))!;

        public async Task<User> GetUserByUsername(string username)
            => (await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Username == username && x.IsActived))!;

        public async Task<User> GetUserByUsernameAndPassword(string username, string password, string? include = null!)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(username));
            IQueryable<User> query = _context.Users;

            if (!string.IsNullOrEmpty(include))
            {
                foreach (var item in include.Split(","))
                {
                    query = query.Include(item);
                }
            }
            return (await query.FirstOrDefaultAsync(x => x.Username == username && x.Password == password && x.IsActived))!;
        }

        public async Task<List<User>> GetUsers(List<Guid> userIds, string? include = null!)
        {
            IQueryable<User> query = _context.Users;

            if (!string.IsNullOrEmpty(include))
            {
                foreach (var item in include.Split(","))
                {
                    query = query.Include(item);
                }
            }
            return await query.Where(x => userIds.Contains(x.Id)).ToListAsync();
        }

        public async Task<bool> UpdateUser(User user)
        {
            Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
