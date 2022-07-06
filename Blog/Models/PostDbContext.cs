using Microsoft.EntityFrameworkCore;

namespace Blog.Models
{
    public class PostDbContext : DbContext
    {
        public PostDbContext(DbContextOptions options):base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
    }
}
