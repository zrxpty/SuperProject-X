using Identity.BLL.Inrefaces;
using Identity.BLL.Interface;
using Identity.BLL.Services;
using Identity.Data;
using JwtAuthenticationManager;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCustomJwtAuthentication();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<IdentityDbContext>(o => o.UseNpgsql(connectionString));

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IRepositotyService, RepositoryService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//using (var scope = app.Services.CreateScope())
//{
//    await using (var db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>())
//    {
//        await db.Database.MigrateAsync();
//    }
//}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "myappname v1"));

}
else
    app.UseHttpsRedirection();


app.UseRouting();

app.MapGet("/", () => "qweqweqweqweqweqeqweqwe12222222222222");

app.UseAuthorization();

app.MapControllers();

app.Run();
