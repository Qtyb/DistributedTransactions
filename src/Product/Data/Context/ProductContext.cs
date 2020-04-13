using Microsoft.EntityFrameworkCore;
using ProductApi.Data.Entities;

namespace ProductApi.Data.Context
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}