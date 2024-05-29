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
    public class ActionService : TokenService, IActionService
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionMultiplexer _connectionMutiple;
        private readonly IDistributedCache _cache;
        private readonly IUnitOfWork _uow;
        private readonly IActionRepository _actionRepository;
        private readonly RedisDTO _redisDTO = new RedisDTO();
        private readonly RedisConstantDTO _redisConstantDTO = new();
        public ActionService(IUnitOfWork unitOfWork, IConfiguration configuration, IConnectionMultiplexer connectionMultiplexer, IDistributedCache distributedCache, IHttpContextAccessor httpContext) : base(unitOfWork, configuration, httpContext, connectionMultiplexer, distributedCache)
        {
            _configuration = configuration;
            _connectionMutiple = connectionMultiplexer;
            _cache = distributedCache;
            _configuration.GetSection(nameof(RedisDTO)).Bind(_redisDTO);
            _configuration.GetSection(nameof(RedisConstantDTO)).Bind(_redisConstantDTO);
            _uow = unitOfWork;
            _actionRepository = _uow.ActionRepository;
        }

        public async Task<List<ActionDTO>> GetActionDTOsCache()
        {
            if (_redisDTO == null!) throw new ArgumentNullException("Can't bind redis server");
            if (_connectionMutiple.GetServer(_redisDTO.Address, _redisDTO.Port).IsConnected)
            {
                var actionString = await _cache.GetStringAsync(_redisConstantDTO.ActionsResource);

                if (actionString != null)
                {
                    return JsonConvert.DeserializeObject<List<ActionDTO>>(actionString)!;
                }
            }
            var actions = await _actionRepository.GetAll().ToListAsync();
            if (actions == null! || actions.Count == 0) throw new ArgumentNullException("Not found user authorize");
            return AutoMapperConfig.Mapper.Map<List<ActionDTO>>(actions);
        }

        public async Task<ClaimDTO> GetClaims(string? token = null)
        {
            if (string.IsNullOrEmpty(token))
            {
                return await GetClaimDTOByAccessToken(token);
            }
            return await GetClaimDTOByAccessToken();
        }
        public async Task InvalidActionCache()
        {
            await _cache.RemoveAsync(_redisConstantDTO.ActionsResource);
        }
        public async Task SaveActionsCache()
        {
            var actions = await _actionRepository.GetAll().ToListAsync();
            if (actions == null! || actions.Count == 0) throw new ArgumentNullException("Not found user authorize");
            await _cache.SetStringAsync(_redisConstantDTO.ActionsResource, JsonConvert.SerializeObject(AutoMapperConfig.Mapper.Map<List<ActionDTO>>(actions)));
        }
    }
}
