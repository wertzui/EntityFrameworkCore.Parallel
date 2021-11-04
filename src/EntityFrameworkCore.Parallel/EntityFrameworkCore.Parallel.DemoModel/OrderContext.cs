using Microsoft.EntityFrameworkCore;
using System;

namespace EntityFrameworkCore.Parallel.DemoModel
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options)
            :base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Detail> Details { get; set; }
    }
}
