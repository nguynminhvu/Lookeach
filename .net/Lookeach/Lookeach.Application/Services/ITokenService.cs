using Lookeach.Core.Entities;

namespace Lookeach.Application.Services
{
    public interface ITokenService
    {
        Task<string> GenerateToken(User user);
    }
}
