using Lookeach.Application.Services;
using Lookeach.Core.DTOs;
using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Lookeach.Core.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

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

        public AuthService(IUnitOfWork unitOfWork, IAesService aesService, IConfiguration configuration, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, configuration, httpContextAccessor, connectionMultiplexer, distributedCache)
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

        public async Task<User> CreateUser(UserCreateModel userCreateModel, Guid roleId)
            => await _userRepository.CreateUser(userCreateModel, roleId);


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
            var tokenCt = _httpAccessor.HttpContext.Request.Headers["Authorization"].First()!.Split(" ").Last();
            return await DeleteAccessToken(tokenCt);
        }

        public async Task<string> RenewToken(string refreshToken)
        {
            var rs = await RenewAccessToken(refreshToken);
            if (rs == null) throw new Exception("Can't renew token");
            return rs;
        }
    }
}
