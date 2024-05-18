using Lookeach.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Lookeach.Infrastructure.Services
{
    public class IdentityUser<T> : TokenService, Application.Interfaces.Services.IIdentityUser<T>
    {
        public IdentityUser(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor) { }

        public T UserId
        {
            get
            {
                var claim = GetClaimDTOByAccessTokenSynchronous();
                Guid userId = Guid.Empty;
                if (claim != null!)
                {
                    userId = claim.Id;
                }
                return (T)Convert.ChangeType(userId, typeof(T));
            }
        }
    }
}
