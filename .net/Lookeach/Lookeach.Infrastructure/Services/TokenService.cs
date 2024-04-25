using Lookeach.Application.Services;
using Lookeach.Core.Entities;
using Lookeach.Core.UnitOfWorks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lookeach.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _uow;

        public TokenService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
            _uow = unitOfWork;
        }

        public async Task<string> GenerateToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSecret"] + "");
            var tokenDescribe = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[] {
                new Claim("Id",user.Id+""),
                new Claim("Name",user.Name),
                new Claim("Roles",JsonConvert.SerializeObject(user.UserRoles)),
                new Claim("Claims", JsonConvert.SerializeObject(user.UserClaims))
                })
            };
            var tokenHandle = new JwtSecurityTokenHandler();
            var token = tokenHandle.CreateToken(tokenDescribe);
            await Task.CompletedTask;
            return tokenHandle.WriteToken(token);
        }

    }
}
