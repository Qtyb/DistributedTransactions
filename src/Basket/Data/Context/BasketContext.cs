using BasketApi.Data.Entites;
using BasketApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BasketApi.Data.Context
{
    public class BasketContext : DbContext
    {
        public BasketContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OutboxEvent> OutboxEvents { get; set; }
    }
}