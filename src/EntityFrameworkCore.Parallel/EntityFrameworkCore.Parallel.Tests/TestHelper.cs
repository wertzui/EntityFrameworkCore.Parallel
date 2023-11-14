using EntityFrameworkCore.Parallel.DemoModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Parallel.Tests;

public static class TestHelper
{
    public static async Task<IDbContextFactory<OrderContext>> CreateFreshDatabase(int entitiesInEachTable)
    {
        var services = new ServiceCollection();
        services.AddPooledDbContextFactory<OrderContext>(options => options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=OrderTest"));

        var provider = services.BuildServiceProvider();

        var factory = provider.GetRequiredService<IDbContextFactory<OrderContext>>();
        using (var context = factory.CreateDbContext())
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var orders = new List<Order>();
            for (int i = 0; i < entitiesInEachTable; i++)
            {
                var order = new Order { OrderNumber = $"O{i}" };
                orders.Add(order);
                for (int j = 0; j < entitiesInEachTable; j++)
                {
                    var detail = new Detail { Order = order, Product = $"P{i}-{j}", Quantity = i * j };
                    order.Details.Add(detail);
                }
            }

            context.AddRange(orders);
            await context.SaveChangesAsync();
        }

        return factory;
    }
}
