using JwtAuthenticationManager;
using Microsoft.AspNetCore.Authentication;
using OtherServer.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
builder.Services.AddMvc();
// Add services to the container.
builder.Services.AddCors();

builder.Services.AddCustomJwtAuthentication();

builder.Services.AddGrpc();
var app = builder.Build();

// Configure the HTTP request pipeline
app.UseCors(policy =>
{
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});
app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<CalculationService>();

app.MapGet("/", () => "Hello, World!");
await app.RunAsync();
