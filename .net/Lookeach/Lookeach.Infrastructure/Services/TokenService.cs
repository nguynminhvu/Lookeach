using Lookeach.Application.Services;
using Lookeach.Core.DTOs;
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
        private JwtDTO _jwtDTO = new();

        public TokenService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
            _uow = unitOfWork;
            _configuration.GetSection(nameof(JwtDTO)).Bind(_jwtDTO);
        }

        public async Task<string> GenerateToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_jwtDTO.JwtSecret);
            var tokenDescribe = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id + ""),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.Name)
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Audience = _jwtDTO.Audience,
                Issuer = _jwtDTO.Issuer,
                Expires = DateTime.Now.AddMinutes(_jwtDTO.ExpireMinute),
            };


            var tokenHandle = new JwtSecurityTokenHandler();
            var token = tokenHandle.CreateToken(tokenDescribe);
            await Task.CompletedTask;
            return tokenHandle.WriteToken(token);
        }

    }
}
