using Lookeach.Application.Services;
using Lookeach.Core.DTOs;
using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Lookeach.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lookeach.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IAesService _aesService;

        public AuthService(IUnitOfWork unitOfWork, IAesService aesService, IConfiguration configuration)
        {
            _uow = unitOfWork;
            _userRepository = _uow.UserRepository;
            _configuration = configuration;
            _aesService = aesService;
        }

        public async Task<string> Login(LoginDTO loginDTO)
        {
            var user = await _userRepository.GetUserByEmailAndPassword(loginDTO.Username, Convert.ToBase64String(_aesService.Encrypt(loginDTO.Password)), "UserRoles,UserClaims");
            if (user == null!)
            {
                user = await _userRepository.GetUserByUsernameAndPassword(loginDTO.Username, Convert.ToBase64String(_aesService.Encrypt(loginDTO.Password)), "UserRoles,UserClaims");
            }
            if (user == null)
                throw new Exception("Wrong username or password");
            return GenerateToken(user);
        }

        private string GenerateToken(User user)
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
            return tokenHandle.WriteToken(token);
        }
    }
}
