using BasketApi.Data.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketApi.Data.Context
{
    public class BasketContext : DbContext
    {
        public BasketContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Basket> Baskets { get; set; }

    }
}
