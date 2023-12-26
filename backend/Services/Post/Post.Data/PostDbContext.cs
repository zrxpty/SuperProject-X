using Microsoft.EntityFrameworkCore;

namespace Post.Data
{
    public class PostDbContext : DbContext
    {
        public DbSet<Models.Post> Posts {  get; set; } 
        public PostDbContext(DbContextOptions<PostDbContext> options) : base(options)
        {

        }
    }
}
