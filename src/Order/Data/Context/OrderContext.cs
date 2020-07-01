using Microsoft.EntityFrameworkCore;
using OrderApi.Data.Entities;

namespace OrderApi.Data.Context
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<OutboxEvent> OutboxEvents { get; set; }
    }
}