using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
         
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
