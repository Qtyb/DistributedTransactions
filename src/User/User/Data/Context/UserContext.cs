using Microsoft.EntityFrameworkCore;
using UserApi.Data.Entities;

namespace UserApi.Data.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}