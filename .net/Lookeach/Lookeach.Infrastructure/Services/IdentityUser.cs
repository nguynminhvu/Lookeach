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
                if (claim == null!) throw new UnauthorizedAccessException("Login session expired");
                return (T)Convert.ChangeType(claim.Id, typeof(T));
            }
        }
    }
}
