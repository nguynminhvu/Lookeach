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

        public async Task<User> GetUserByEmailAndPassword(string email, string password, string? include = null!)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(email));

            if (!string.IsNullOrEmpty(include))
            {
                foreach (var item in include.Split(","))
                {
                    _context.Users.Include(item);
                }
            }
            return (await _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == password))!;
        }

        public async Task<User> GetUserById(Guid id)
             => (await _context.Users.FirstOrDefaultAsync(x => x.Id == id))!;



        public async Task<User> GetUserByUsernameAndPassword(string username, string password, string? include = null!)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(username));

            if (!string.IsNullOrEmpty(include))
            {
                foreach (var item in include.Split(","))
                {
                    _context.Users.Include(item);
                }
            }
            return (await _context.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password))!;
        }
    }
}
