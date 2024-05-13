using Lookeach.Application.AutoMappers;
using Lookeach.Application.DTOs;
using Lookeach.Application.Services;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Lookeach.Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IConfiguration _configuation;
        private readonly IUnitOfWork _uow;
        private readonly IDistributedCache _cache;
        private readonly IConnectionMultiplexer _connectionMutiple;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly RedisDTO _redisDTO = new();
        private readonly IPermissionRepository _permissionRepository;
        private readonly RedisConstantDTO _redisConstantDTO = new RedisConstantDTO();

        public PermissionService(IUnitOfWork unitOfWork, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _configuation = configuration;
            _uow = unitOfWork;
            _cache = distributedCache;
            _connectionMutiple = connectionMultiplexer;
            _httpAccessor = httpContextAccessor;
            _configuation.GetSection(nameof(RedisDTO)).Bind(_redisDTO);
            _configuation.GetSection(nameof(RedisConstantDTO)).Bind(_redisConstantDTO);
            _permissionRepository = _uow.PermissionRepository;
        }

        public async Task<List<PermissionDTO>> GetPermissionsCache(Guid roleId)
        {
            if (_connectionMutiple.GetServer(_redisDTO.Address, _redisDTO.Port).IsConnected)
            {
                var permissionString = await _cache.GetStringAsync(_redisConstantDTO.PermissionResource);
                if (permissionString != null)
                {
                    var permissions = JsonConvert.DeserializeObject<List<PermissionDTO>>(permissionString);
                    if (permissions != null!)
                    {
                        return permissions.Where(x => x.RoleId == roleId).ToList();
                    }
                }
            }
            return AutoMapperConfig.Mapper.Map<List<PermissionDTO>>(await _permissionRepository.GetMany(x => x.RoleId == roleId, "Role,Resource").ToListAsync());
        }

        public async Task<List<PermissionDTO>> GetPermissionsCache()
        {
            if (_connectionMutiple.GetServer(_redisDTO.Address, _redisDTO.Port).IsConnected)
            {
                var permissionString = await _cache.GetStringAsync(_redisConstantDTO.PermissionResource);
                if (permissionString != null)
                {
                    return JsonConvert.DeserializeObject<List<PermissionDTO>>(permissionString)!;
                }
            }
            return AutoMapperConfig.Mapper.Map<List<PermissionDTO>>(await _permissionRepository.GetPermissions());
        }

        public async Task SavePermissionCache()
        {
            var permission = await GetPermissionsCache();
            var json = JsonConvert.SerializeObject(AutoMapperConfig.Mapper.Map<List<PermissionDTO>>(permission));
            await _cache.SetStringAsync(_redisConstantDTO.PermissionResource, json);
        }

        public Task<bool> ValidPermission(PermissionDTO permissionDTO, string resourceAccess)
        {
            throw new NotImplementedException();
        }
    }
}
