using Lookeach.Core.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Lookeach.Extensions.Jwt
{
    public static class JwtConfiguration
    {
        public static void AddLookeachJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                JwtDTO jwtDTO = new JwtDTO();
                configuration.GetSection(nameof(JwtDTO)).Bind(jwtDTO);
                o.SaveToken = true; // For get token in HttpContext later if I need

                var key = Encoding.UTF8.GetBytes(jwtDTO.JwtSecret);

                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = false, // Turn on when run in production
                    ValidateAudience = false, // Same
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtDTO.Issuer,
                    ValidAudience = jwtDTO.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
                o.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = x =>
                    {
                        if (x.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            x.Response!.Headers!.TryAdd("IS-LOOKEACH-TOKEN-EXPIRED", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddAuthorization();
        }
    }
}
