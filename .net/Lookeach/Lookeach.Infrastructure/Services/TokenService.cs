using Lookeach.Application.Services;
using Lookeach.Core.DTOs;
using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Lookeach.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMutiple;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly IAccessTokenRepository _accessTokenRepository;
        private JwtDTO _jwtDTO = new();
        private RefreshTokenDTO _refreshTokenDTO = new();
        private RedisDTO _redisDTO = new();

        public TokenService()
        {

        }

        public TokenService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContext, IConnectionMultiplexer connectionMultiplexer, IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            _connectionMutiple = connectionMultiplexer;
            _httpAccessor = httpContext;
            _configuration = configuration;
            _uow = unitOfWork;
            _configuration.GetSection(nameof(JwtDTO)).Bind(_jwtDTO);
            _configuration.GetSection(nameof(RedisDTO)).Bind(_redisDTO);
            _configuration.GetSection(nameof(RefreshTokenDTO)).Bind(_refreshTokenDTO);
            _userRepository = _uow.UserRepository;
            _accessTokenRepository = _uow.AccessTokenRepository;
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
        public async Task<bool> CheckRefreshTokenExpire(string token)
        {
            var rt = await _accessTokenRepository.GetAccessTokenByRefreshToken(token);
            if (rt == null) throw new UnauthorizedAccessException("Refresh token invalid");
            var time = rt.CreatedOn.AddMinutes(_refreshTokenDTO.ExpireMinute);
            if (time < DateTime.Now)
            {
                return false;
            }
            return true;
        }

        public async Task<ClaimDTO> GetClaimDTOByAccessToken(string token)
        {
            try
            {
                var keyExample = _jwtDTO.JwtSecret;
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
                throw new Exception(ex.Message);
            }
        }
        public string GenerateRefreshToken()
        {
            var rdr = new byte[32];
            using (var rd = RandomNumberGenerator.Create())
            {
                rd.GetBytes(rdr);
                return Convert.ToBase64String(rdr);
            }
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

        public async Task SaveAccessToken(string token, Guid userId)
        {
            await _distributedCache.SetStringAsync(token, userId + "");
            await _accessTokenRepository.AddAccessToken(token, userId, GenerateRefreshToken());
        }


        public async Task<bool> DeleteAccessToken(string token)
        {
            await _distributedCache.RemoveAsync(token);
            await _accessTokenRepository.RemoveTokenByToken(token);
            return true;
        }

        public async Task<string> RenewAccessToken(string refreshToken)
        {
            var token = await _accessTokenRepository.GetAccessTokenByRefreshToken(refreshToken);
            if (token == null!)
            {
                throw new UnauthorizedAccessException("Invalid token, login please");
            }
            if (token.ModifiedOn.AddMinutes(_jwtDTO.ExpireMinute) > DateTime.Now)
            {
                throw new UnauthorizedAccessException("Token still valid, can't renew token");
            }
            if (await CheckRefreshTokenExpire(token.RefreshToken) is false)
            {
                await DeleteAccessToken(token.RefreshToken);
                throw new SecurityTokenExpiredException("Refresh token expired, login again");
            }
            var user = await _userRepository.GetUserById(token.CreatedBy);
            if (user == null)
            {
                throw new ArgumentNullException("Not found user");
            }
            string oldToken = token.Token;
            var rs = await _accessTokenRepository.UpdateTokenByRefreshToken(await GenerateToken(user), refreshToken);
            if (rs != null)
            {
                await _distributedCache.RemoveAsync(oldToken);
                await _distributedCache.SetStringAsync(rs.Token, rs.ModifiedBy + "");
                return rs.Token;
            }
            return null!;
        }
    }
}
