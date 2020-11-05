using EntitiyFrameworkCore.Parallel;
using EntityFrameworkCore.Parallel.DemoModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Parallel.Tests
{
    [TestClass]
    public class DatabaseTests
    {
        private IDbContextFactory<OrderContext> _factory;

        [TestInitialize]
        public async Task TestInitialize()
        {
            var services = new ServiceCollection();
            services.AddPooledDbContextFactory<OrderContext>(options => options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=OrderTest"));

            var provider = services.BuildServiceProvider();

            _factory = provider.GetRequiredService<IDbContextFactory<OrderContext>>();
            using (var context = _factory.CreateDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();

                var orders = new List<Order>();
                for (int i = 0; i < 10; i++)
                {
                    var order = new Order { OrderNumber = $"O{i}" };
                    orders.Add(order);
                    for (int j = 0; j < 10; j++)
                    {
                        var detail = new Detail { Order = order, Product = $"P{i}-{j}", Quantity = i * j };
                        order.Details.Add(detail);
                    }
                }

                context.AddRange(orders);
                await context.SaveChangesAsync();
            }
        }

        [TestMethod]
        public async Task ToList_should_return_the_Orders_from_the_database()
        {
            // Act
            var ordersFromDb = _factory.Set<OrderContext, Order>().Where(o => o.Id > 3).ToList();

            // Assert
            Assert.AreEqual(7, ordersFromDb.Count);
        }

        [TestMethod]
        public async Task ToListAsync_should_return_the_Orders_from_the_database()
        {
            // Act
            var ordersFromDb = await _factory.Set<OrderContext, Order>().Where(o => o.Id > 3).ToListAsync();

            // Assert
            Assert.AreEqual(7, ordersFromDb.Count);
        }

        [TestMethod]
        public async Task First_should_return_the_first_Order_from_the_database()
        {
            // Act
            var orderFromDb = _factory.Set<OrderContext, Order>().First();

            // Assert
            Assert.IsNotNull(orderFromDb);
        }

        [TestMethod]
        public async Task FirstAsync_should_return_the_first_Order_from_the_database()
        {
            // Act
            var orderFromDb = await _factory.Set<OrderContext, Order>().FirstAsync();

            // Assert
            Assert.IsNotNull(orderFromDb);
        }
    }
}
