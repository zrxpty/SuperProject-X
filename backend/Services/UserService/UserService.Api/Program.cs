using Microsoft.EntityFrameworkCore;
using UserService.BLL.Intrefaces;
using UserService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<UsersDbContext>(o => o.UseNpgsql(connectionString));


builder.Services.AddScoped<IUserService, UserService.BLL.Services.UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    await using (var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>())
    {
        await db.Database.MigrateAsync();
    }
}

app.UseAuthorization();

app.MapControllers();

app.Run();
