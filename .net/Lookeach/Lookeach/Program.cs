using Lookeach.Exceptions;
using Lookeach.Extensions.Exceptions;
using Lookeach.Extensions.Injects;
using Lookeach.Extensions.Jwt;
using Lookeach.Extensions.SwaggerOption;
using Lookeach.Infrastructure.Extensions;
using Newtonsoft.Json.Serialization;
using System.Text.Json.Serialization;

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
builder.Services.InfrastructureInject(builder.Configuration);
builder.Services.InjectDependency(builder.Configuration);
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
app.UseABAC();
app.UseAuthorization();
app.UseLookeachException();

app.MapControllers();

app.Run();
