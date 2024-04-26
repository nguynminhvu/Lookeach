using Lookeach.Extensions.Jwt;
using Lookeach.Application.Extensions;
using Lookeach.Infrastructure.Extensions;
using Lookeach.Extensions.SwaggerOption;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerLookeach(builder.Configuration);

builder.Services.AddLookeachJwt(builder.Configuration);
builder.Services.ApplicationInject(builder.Configuration);
builder.Services.InfrastructureInject(builder.Configuration);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<JwtRedis>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
