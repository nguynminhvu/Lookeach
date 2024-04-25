using Lookeach.Core.Entities;
using Lookeach.Core.Repositories.Base;

namespace Lookeach.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByUsernameAndPassword(string username, string password, string? include = null!);
        Task<User> GetUserByEmailAndPassword(string email, string password, string? include = null!);
        Task<User> GetUserById(Guid id);
    }
}
