using Lookeach.Application.Services;
using Lookeach.Core.DTOs;
using Lookeach.Core.UnitOfWorks;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Lookeach.Infrastructure.Extensions
{
    public static class DI
    {
        public static void InfrastructureInject(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LookeachContext>(x => x.UseSqlServer(GetConnectionString()));
            services.AddScoped<IUnitOfWork, Lookeach.Infrastructure.UnitOfWork.UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAesService, AesService>();

            var redisDto = new RedisDTO();
            configuration.GetSection(nameof(RedisDTO)).Bind(redisDto);
            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(redisDto.Address));
            services.AddStackExchangeRedisCache(x => x.Configuration = redisDto.Address);

        }

        private static string GetConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, true).Build();
            return configuration["ConnectionStrings:LookeachDb"] + "";
        }
    }
}
