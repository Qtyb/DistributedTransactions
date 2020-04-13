using Microsoft.EntityFrameworkCore;
using OrderApi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Data.Context
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }

    }
}
