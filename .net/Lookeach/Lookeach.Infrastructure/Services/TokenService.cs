using Lookeach.Application.Services;
using Lookeach.Core.DTOs;
using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Lookeach.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IAccessTokenRepository _accessTokenRepository;
        private JwtDTO _jwtDTO = new();
        private RefreshTokenDTO _refreshTokenDTO;
        public TokenService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContext, IAuthService authService)
        {
            _authService = authService;
            _httpAccessor = httpContext;
            _configuration = configuration;
            _uow = unitOfWork;
            _configuration.GetSection(nameof(JwtDTO)).Bind(_jwtDTO);
            _configuration.GetSection(nameof(RefreshTokenDTO)).Bind(_refreshTokenDTO);
            _userRepository = _uow.UserRepository;
            _refreshTokenRepository = _uow.RefreshTokenRepository;
            _accessTokenRepository = _uow.AccessTokenRepository;
        }

        public async Task<bool> CheckRefreshTokenExpire(string refreshToken)
        {
            var rt = await _refreshTokenRepository.GetRefreshToken(refreshToken);
            if (rt == null) throw new UnauthorizedAccessException("Refresh token invalid");
            var time = rt.CreatedOn.AddMinutes(_refreshTokenDTO.ExpireMinute);
            if (time < DateTime.Now)
            {
                return false;
            }
            return true;
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

        public async Task<RefreshToken> AddRefreshToken(string rt)
        {
            var token = _httpAccessor.HttpContext.Request.Headers["Authorize"].First()!.Split(" ").Last();
            if (token == null)
            {
                throw new UnauthorizedAccessException("No token in header");
            }
            var claims = await _authService.GetClaimDTOByAccessToken(token);
            var user = await _userRepository.GetUserById(claims.Id);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Not found user");
            }
            await _refreshTokenRepository.AddRefreshToken(rt, user.Id);
            return await _refreshTokenRepository.GetRefreshToken(rt);
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
            await _accessTokenRepository.AddAccessToken(token, userId);
        }

        public async Task<string> GetAccessTokenFromRequest()
        {
            var token = _httpAccessor.HttpContext.Request.Headers["Authorize"].First()!.Split(" ").Last();
            if (token == null)
            {
                throw new UnauthorizedAccessException("No token in header");
            }
            var claims = await _authService.GetClaimDTOByAccessToken(token);
            var user = await _userRepository.GetUserById(claims.Id);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Not found user");
            }
            return token;
        }

        public async Task<bool> DeleteAccessToken(string token)
        {
            await _accessTokenRepository.RemoveTokenByToken(token);
            return true;
        }
    }
}
