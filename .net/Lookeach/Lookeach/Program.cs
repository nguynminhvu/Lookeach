using Lookeach.Extensions.Jwt;
using Lookeach.Application.Extensions;
using Lookeach.Infrastructure.Extensions;
using Lookeach.Extensions.SwaggerOption;
using Lookeach.Extensions.Exceptions;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Lookeach.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(x =>
{
    x.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    x.Filters.Add(typeof(ExceptionHandler));
}).AddNewtonsoftJson(x =>
{
    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
}).AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

});

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
app.UseRouting();
app.UseAuthentication();
app.UseMiddleware<JwtRedis>();
app.UseAuthorization();
app.UseLookeachException();
app.MapControllers();

app.Run();
