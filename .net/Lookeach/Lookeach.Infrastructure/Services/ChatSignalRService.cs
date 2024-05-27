using Lookeach.Application.AutoMappers;
using Lookeach.Application.DTOs;
using Lookeach.Application.Features.Chats.Models;
using Lookeach.Application.Interfaces;
using Lookeach.Application.Interfaces.Services;
using Lookeach.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Lookeach.Infrastructure.Services
{
    public class ChatSignalRService : IChatSignalRService
    {
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly IConfiguration _configuration;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly ICleanContext _cleanContext;
        private readonly IDistributedCache _cache;
        private readonly RedisDTO _redisDTO = new RedisDTO();
        public ChatSignalRService(ICleanContext cleanContext, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer, IConfiguration configuration, IIdentityUser<Guid> identityUser)
        {
            _identityUser = identityUser;
            _configuration = configuration;
            _connectionMultiplexer = connectionMultiplexer;
            _cleanContext = cleanContext;
            _cache = distributedCache;
            _configuration.GetSection(nameof(RedisDTO)).Bind(_redisDTO);
        }

        public async Task<List<ChatViewModel>> GetChatCache()
        {
            if (_identityUser.UserId == Guid.Empty) throw new UnauthorizedAccessException("Login session expired");
            if (_connectionMultiplexer.GetServer(_redisDTO.Address, _redisDTO.Port).IsConnected)
            {
                var chatMsg = await _cache.GetStringAsync("ChatPerson" + _identityUser.UserId);
                if (!string.IsNullOrEmpty(chatMsg))
                {
                    return AutoMapperConfig.Mapper.Map<List<ChatViewModel>>(chatMsg);
                }
            }
            var chatIds = await _cleanContext.ParticipantChats.Where(x => x.SenderId == _identityUser.UserId).Select(x => x.ChatId).ToListAsync();
            var chatmsg = await _cleanContext.Chats.Where(x => chatIds.Contains(x.Id)).ToListAsync();
            var chatVm = AutoMapperConfig.Mapper.Map<List<ChatViewModel>>(chatmsg);
            await _cache.SetStringAsync("ChatPerson" + _identityUser.UserId, JsonConvert.SerializeObject(chatVm));
            return chatVm;
        }
    }
}
