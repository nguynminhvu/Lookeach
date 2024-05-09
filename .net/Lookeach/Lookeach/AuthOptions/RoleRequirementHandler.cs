using Lookeach.Application.Services;
using Lookeach.AuthModels;
using Lookeach.Core.DTOs;
using Lookeach.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text;

namespace Lookeach.AuthOptions
{
    public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly IActionService _actionService;
        private readonly IConfiguration _configuration;
        private readonly IConnectionMultiplexer _connectionMutiple;
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpAccessor;
        private JwtDTO _jwtDTO = new JwtDTO();

        public RoleRequirementHandler(IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache, IConfiguration configuration, IConnectionMultiplexer connectionMultiplexer, IActionService actionService)
        {
            _actionService = actionService;
            _configuration = configuration;
            _connectionMutiple = connectionMultiplexer;
            _cache = distributedCache;
            _httpAccessor = httpContextAccessor;
            _configuration.GetSection(nameof(JwtDTO)).Bind(_jwtDTO);
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
        private async Task<bool> ValidActionResource(string action, int roleValue)
        {
            List<ActionDTO> actionDTOs = await _actionService.GetActionDTOs();

            if (actionDTOs?.Any() != true) return false;

            actionDTOs = actionDTOs.OrderBy(x => x.Index).ToList();
            char[] authorizeString = ReverseString(DecimalToBinary(roleValue)).ToArray();

            Dictionary<string, bool> actionKey = new Dictionary<string, bool>();
            int i = 0;
            foreach (var item in authorizeString)
            {
                bool vlue;
                if (item == '0')
                {
                    vlue = false;
                }
                else if (item == '1')
                {
                    vlue = true;
                }
                else
                {
                    return false;
                }
                actionKey.Add(actionDTOs[i].Name, vlue);
            }
            if (actionKey.ContainsKey(action))
            {
                return true;
            }
            return false;
        }

        private string DecimalToBinary(int decimalNumber)
        {
            if (decimalNumber == 0)
                return "0";

            StringBuilder binary = new StringBuilder();

            while (decimalNumber > 0)
            {
                binary.Insert(0, decimalNumber % 2);
                decimalNumber /= 2;
            }

            return binary.ToString();
        }

        private char[] ReverseString(string str)
        {
            char[] chars = str.ToCharArray();
            int length = str.Length;
            for (int i = 0; i <= length / 2; i++)
            {
                char temp = chars[i];
                chars[i] = chars[length - 1 - i];
                chars[length - 1 - i] = temp;
            }
            return chars;
        }
    }
}
