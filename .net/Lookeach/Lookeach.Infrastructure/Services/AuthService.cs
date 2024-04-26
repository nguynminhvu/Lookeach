using Lookeach.Application.Services;
using Lookeach.Core.DTOs;
using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Lookeach.Core.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Lookeach.Infrastructure.Services
{
    public class AuthService : TokenService, IAuthService
    {
        private readonly IConnectionMultiplexer _connectionMutiple;
        private readonly IDistributedCache _distributedCache;
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IAesService _aesService;
        private readonly IHttpContextAccessor _httpAccessor;
        private JwtDTO _jwtDTO;
        private RedisDTO _redisDTO;

        public AuthService(IUnitOfWork unitOfWork, IAesService aesService, IConfiguration configuration, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer, IHttpContextAccessor httpContextAccessor)
        {
            _httpAccessor = httpContextAccessor;
            _connectionMutiple = connectionMultiplexer;
            _distributedCache = distributedCache;
            _uow = unitOfWork;
            _userRepository = _uow.UserRepository;
            _configuration = configuration;
            _aesService = aesService;
            _configuration.GetSection(nameof(JwtDTO)).Bind(_jwtDTO);
            _configuration.GetSection(nameof(RedisDTO)).Bind(_redisDTO);
        }

        public Task<User> CreateUserRole(UserCreateModel userCreateModel)
        {
            throw new NotImplementedException();
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
                    ValidateIssuer = false, //**
                    ValidateAudience = false, //** 
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = _jwtDTO.Issuer,
                    ValidAudience = _jwtDTO.Audience,

                }, out SecurityToken securityToken);
                var jwtToken = (JwtSecurityToken)securityToken;
                var id = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
                var email = jwtToken.Claims.First(x => x.Type == ClaimTypes.Email)!.Value;
                var role = jwtToken.Claims.First(x => x.Type == ClaimTypes.Role)!.Value;

                if (Guid.TryParse(id, out Guid newId) is false ||
                    string.IsNullOrEmpty(email) ||
                    string.IsNullOrEmpty(role)
                    )
                {
                    return null!;
                }
                await Task.CompletedTask;
                return new ClaimDTO
                {
                    Id = newId,
                    Email = email,
                    Role = role
                };
            }
            catch (Exception ex)
            {
                return null!;
            }
        }

        public async Task<bool> GetTokenValid(string token)
        {
            try
            {
                if (_connectionMutiple.GetServer(_redisDTO.Address, _redisDTO.Port).IsConnected)
                {
                    return (await _distributedCache.GetStringAsync(token) + "") != "";
                }
                return (await _uow.AccessTokenRepository.GetAccessTokenByToken(token) + "") != "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> Login(LoginDTO loginDTO)
        {
            var user = await _userRepository.GetUserByEmailAndPassword(loginDTO.Username, Convert.ToBase64String(_aesService.Encrypt(loginDTO.Password)), "Role");
            if (user == null!)
            {
                user = await _userRepository.GetUserByUsernameAndPassword(loginDTO.Username, Convert.ToBase64String(_aesService.Encrypt(loginDTO.Password)), "Role");
            }
            if (user == null)
                throw new Exception("Wrong username or password");
            var tk = await GenerateToken(user);
            if (tk != null)
            {
                await SaveAccessToken(tk, user.Id);
                return tk;
            }
            throw new Exception("Can't create token");
        }

        public async Task<bool> Logout()
        {
            var tokenCt = _httpAccessor.HttpContext.Request.Headers["Authorize"].First()!.Split(" ").Last();
            if (tokenCt == null!)
            {
                throw new UnauthorizedAccessException("Not found user");
            }
            var token = await GetAccessTokenFromRequest(await GetClaimDTOByAccessToken(tokenCt));
            await _distributedCache.RemoveAsync(token);
            await DeleteAccessToken(token);
            return true;
        }
    }
}
