using Lookeach.Application.AutoMappers;
using Lookeach.Core.DTOs;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Lookeach.Infrastructure.Services
{
    public class ActionService
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionMultiplexer _connectionMutiple;
        private readonly IDistributedCache _cache;
        private readonly IUnitOfWork _uow;
        private readonly IActionRepository _actionRepository;
        private readonly RedisDTO _redisDTO = new RedisDTO();

        public ActionService(IUnitOfWork unitOfWork, IConfiguration configuration, IConnectionMultiplexer connectionMultiplexer, IDistributedCache distributedCache)
        {
            _configuration = configuration;
            _connectionMutiple = connectionMultiplexer;
            _cache = distributedCache;
            _configuration.GetSection(nameof(JwtDTO)).Bind(_redisDTO);
            _uow = unitOfWork;
            _actionRepository = _uow.ActionRepository;
        }

        public async Task<List<ActionDTO>> GetActionDTOs()
        {
            if (_redisDTO == null!) throw new ArgumentNullException("Can't bind redis server");
            if (_connectionMutiple.GetServer(_redisDTO.Address, _redisDTO.Port).IsConnected)
            {
                var actionString = await _cache.GetStringAsync("ActionsResource");

                if (actionString != null)
                {
                    return JsonConvert.DeserializeObject<List<ActionDTO>>(actionString)!;
                }
            }
            var actions = await _actionRepository.GetAll().ToListAsync();
            if (actions == null! || actions.Count == 0) throw new ArgumentNullException("Not found user authorize");
            return AutoMapperConfig.Mapper.Map<List<ActionDTO>>(actions);
        }
    }
}
