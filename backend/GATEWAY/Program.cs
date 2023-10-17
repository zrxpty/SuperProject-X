using JwtAuthenticationManager;
using Microsoft.AspNetCore.Cors;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
builder.Services.AddCors();
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddCustomJwtAuthentication();

var app = builder.Build();

app.UseCors(policy =>
{
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});

await app.UseOcelot();
app.MapGet("/", () => "qweqweqweqweqweqeqweqwe12222222222222");
app.UseAuthentication();
app.UseAuthorization();

app.Run();
