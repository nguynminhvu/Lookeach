using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Core.ViewModels.Users;
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

        public async Task<User> CreateUser(UserCreateModel userCreateModel, Guid roleId)
        {
            var id = Guid.NewGuid();
            var user = User
                .CreateUser(id, userCreateModel.Username, userCreateModel.Name, userCreateModel.Dob, userCreateModel.Address,
                userCreateModel.Gender, userCreateModel.Email, userCreateModel.Phone, userCreateModel.Password, roleId, id, id);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

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
            return (await query.FirstOrDefaultAsync(x => x.Email == email && x.Password == password))!;
        }

        public async Task<User> GetUserById(Guid id)
             => (await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id))!;



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
            return (await query.FirstOrDefaultAsync(x => x.Username == username && x.Password == password))!;
        }
    }
}
