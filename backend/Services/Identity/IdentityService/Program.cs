using Identity.BLL.Inrefaces;
using Identity.BLL.Services;
using Identity.Data;
using Identity.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

//var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<IdentityDbContext>(o => o.UseNpgsql(connectionString));

//builder.Services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(connection));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRepositotyService, RepositoryService>();


builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope()) 
{
    await using (var db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>())
    {
        await db.Database.MigrateAsync();
    }
}

// Configure the HTTP request pipeline.
app.MapGrpcService<IdentityServiceGrpc>();
app.MapGet("/", () => "qweqweqweqweqweqeqweqwe12222222222222GRPC");



app.Run();
