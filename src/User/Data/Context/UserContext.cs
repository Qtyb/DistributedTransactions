using Microsoft.EntityFrameworkCore;
using UserApi.Data.Entities;

namespace UserApi.Data.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OutboxEvent> OutboxEvents { get; set; }

    }
}