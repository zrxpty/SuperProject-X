using Microsoft.EntityFrameworkCore;
using UserService.Data.Models;

namespace UserService.Data
{
    public class UsersDbContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }

        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {

        }
    }
}
