using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Parallel.DemoModel
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options)
            : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; } = default!;

        public DbSet<Detail> Details { get; set; } = default!;
    }
}
