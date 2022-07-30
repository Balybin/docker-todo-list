using DockerTodoList.Domain;
using Microsoft.EntityFrameworkCore;

namespace DockerTodoList.Infrastructure.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}