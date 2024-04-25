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
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IAesService _aesService;

        public AuthService(IUnitOfWork unitOfWork, IAesService aesService, IConfiguration configuration, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _uow = unitOfWork;
            _userRepository = _uow.UserRepository;
            _configuration = configuration;
            _aesService = aesService;
        }

        public async Task<ClaimDTO> GetClaimDTOByAccessToken(string token)
        {
            try
            {
                var keyExample = _configuration["JwtSecret"] + "";
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(keyExample)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken securityToken);
                var jwtToken = (JwtSecurityToken)securityToken;
                var id = jwtToken.Claims.First(x => x.Type == "Id")!.Value;
                var name = jwtToken.Claims.First(x => x.Type == "Name")!.Value;
                var roles = jwtToken.Claims.First(x => x.Type == "Roles")!.Value;
                var claims = jwtToken.Claims.First(x => x.Type == "Claims")!.Value;

                if (Guid.TryParse(id, out Guid newId) is false ||
                    string.IsNullOrEmpty(name) ||
                    string.IsNullOrEmpty(roles) ||
                    string.IsNullOrEmpty(claims)
                    )
                {
                    return null!;
                }
                await Task.CompletedTask;
                return new ClaimDTO
                {
                    Id = newId,
                    Name = name,
                    Roles = JsonConvert.DeserializeObject<List<UserRole>>(roles)!,
                    Claims = JsonConvert.DeserializeObject<List<UserClaim>>(claims)!
                };
            }
            catch (Exception ex)
            {
                return null!;
            }
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
            return await _tokenService.GenerateToken(user);
        }

    }
}
