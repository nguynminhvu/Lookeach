using Lookeach.Application.AutoMappers;
using Lookeach.Application.DTOs;
using Lookeach.Application.Services;
using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
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
        private RedisConstantDTO _redisContantDTO = new();
        public TokenService()
        {

        }
        public TokenService(IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            _httpAccessor = httpContext;
            _configuration = configuration;
            _configuration.GetSection(nameof(JwtDTO)).Bind(_jwtDTO);
            _configuration.GetSection(nameof(RedisDTO)).Bind(_redisDTO);
            _configuration.GetSection(nameof(RefreshTokenDTO)).Bind(_refreshTokenDTO);
            _configuration.GetSection(nameof(RedisConstantDTO)).Bind(_redisContantDTO);
        }
        public TokenService(IConfiguration configuration, IHttpContextAccessor httpContext, IConnectionMultiplexer connectionMultiplexer, IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            _connectionMutiple = connectionMultiplexer;
            _httpAccessor = httpContext;
            _configuration = configuration;
            _configuration.GetSection(nameof(JwtDTO)).Bind(_jwtDTO);
            _configuration.GetSection(nameof(RedisDTO)).Bind(_redisDTO);
            _configuration.GetSection(nameof(RefreshTokenDTO)).Bind(_refreshTokenDTO);
            _configuration.GetSection(nameof(RedisConstantDTO)).Bind(_redisContantDTO);
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
            _configuration.GetSection(nameof(RedisConstantDTO)).Bind(_redisContantDTO);
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

        public async Task<ClaimDTO> GetClaimDTOByAccessToken(string? token = null!)
        {
            try
            {
                if (token == null)
                {
                    token = (_httpAccessor.HttpContext.Request.Headers["Authorization"].First() + "").Split(" ").Last();
                }
            }
            catch
            {
                throw new UnauthorizedAccessException("Login session has expired");
            }
            if (token == null!) throw new UnauthorizedAccessException("Not found token, login please");
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
                var id = jwtToken.Claims.First(x => x.Type == "nameid")!.Value;
                var email = jwtToken.Claims.First(x => x.Type == "email")!.Value;

                if (Guid.TryParse(id, out Guid newId) is false ||
                    string.IsNullOrEmpty(email)
                    )
                {
                    return null!;
                }
                await Task.CompletedTask;
                return new ClaimDTO
                {
                    Id = newId,
                    Email = email
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ClaimDTO GetClaimDTOByAccessTokenSynchronous(string? token = null!)
        {
            try
            {
                if (token == null)
                {
                    token = (_httpAccessor.HttpContext.Request.Headers["Authorization"].First() + "").Split(" ").Last();
                }
            }
            catch
            {
                return null!;
            }
            if (token == null!) return null!;
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
                var id = jwtToken.Claims.First(x => x.Type == "nameid")!.Value;
                var email = jwtToken.Claims.First(x => x.Type == "email")!.Value;
                if (Guid.TryParse(id, out Guid newId) is false ||
                    string.IsNullOrEmpty(email)
                    )
                {
                    return null!;
                }

                return new ClaimDTO
                {
                    Id = newId,
                    Email = email
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
                    new Claim(ClaimTypes.Email, user.Email)
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
            var claim = await GetClaimDTOByAccessToken();
            if (claim == null!)
            {
                throw new UnauthorizedAccessException();
            }
            var rs = await _accessTokenRepository.UpdateTokenByRefreshToken(await GenerateToken(user), refreshToken, claim.Id);
            if (rs != null)
            {
                await _distributedCache.RemoveAsync(oldToken);
                await _distributedCache.SetStringAsync(rs.Token, rs.ModifiedBy + "");
                return rs.Token;
            }
            return null!;
        }

        public async Task<string> GenerateToken(SuperAdmin user)
        {
            var key = Encoding.UTF8.GetBytes(_jwtDTO.JwtSecret);
            var tokenDescribe = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id + "")
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

        public async Task SaveUserRoleCache(Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null!) throw new Exception("User not found");
            await _distributedCache.SetStringAsync(_redisContantDTO.UserRoleResource + user.Id, JsonConvert.SerializeObject(user.Roles, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            }));
        }

        public async Task InvalidUserRoleCache(Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null!) throw new Exception("User not found");
            await _distributedCache.RemoveAsync(_redisContantDTO.UserRoleResource + user.Id);
        }

        public async Task<List<Core.Entities.Role>> GetRoleByUserIdCache(Guid id)
        {
            List<Core.Entities.Role> roles = new List<Core.Entities.Role>();
            try
            {
                if (_connectionMutiple.GetServer(_redisDTO.Address, _redisDTO.Port).IsConnected)
                {
                    string userRoles = await _distributedCache.GetStringAsync(_redisContantDTO.UserRoleResource + id) ;
                    if (userRoles==null!)
                    {
                        var userRole = await _uow.UserRepository.FirstOrDefaultAsync(x => x.Id == id, "Roles");
                        await SaveUserRoleCache(id);
                        return AutoMapperConfig.Mapper.Map<List<Core.Entities.Role>>(userRole.Roles);
                    }
                    return (JsonConvert.DeserializeObject<List<Core.Entities.Role>>(userRoles))!;
                }
                var user = await _uow.UserRepository.FirstOrDefaultAsync(x => x.Id == id, "Roles");
                return AutoMapperConfig.Mapper.Map<List<Core.Entities.Role>>(user.Roles);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
