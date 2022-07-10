using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class InMemoryApplicationDbContext : DbContext
    {
        public InMemoryApplicationDbContext(DbContextOptions options):base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
    }
}
