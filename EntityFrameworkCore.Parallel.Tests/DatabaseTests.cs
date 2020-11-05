using EntitiyFrameworkCore.Parallel;
using EntityFrameworkCore.Parallel.DemoModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            _factory = await TestHelper.CreateFreshDatabase(10);
        }

        [TestMethod]
        public async Task ToList_should_return_the_Orders_from_the_database()
        {
            // Act
            var ordersFromDb = _factory.Set<OrderContext, Order>().ToList();

            // Assert
            Assert.AreEqual(10, ordersFromDb.Count);
        }

        [TestMethod]
        public async Task ToListAsync_should_return_the_Orders_from_the_database()
        {
            // Act
            var ordersFromDb = await _factory.Set<OrderContext, Order>().ToListAsync();

            // Assert
            Assert.AreEqual(10, ordersFromDb.Count);
        }

        [TestMethod]
        public async Task ToList_with_Where_should_return_the_Orders_from_the_database()
        {
            // Act
            var ordersFromDb = _factory.Set<OrderContext, Order>().Where(o => o.Id > 3).ToList();

            // Assert
            Assert.AreEqual(7, ordersFromDb.Count);
        }

        [TestMethod]
        public async Task ToListAsync_with_Where_should_return_the_Orders_from_the_database()
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
