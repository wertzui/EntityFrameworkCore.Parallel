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
        private IDbContextFactory<OrderContext> _factory = default!;

        [TestInitialize]
        public async Task TestInitialize()
        {
            _factory = await TestHelper.CreateFreshDatabase(10);
        }

        [TestMethod]
        public void ToList_should_return_the_Orders_from_the_database()
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
        public void SelectMany_and_ToList_should_return_the_Details_from_the_database()
        {
            // Act
            var detailsFromDb = _factory.Set<OrderContext, Order>().SelectMany(o => o.Details).ToList();

            // Assert
            Assert.AreEqual(100, detailsFromDb.Count);
        }

        [TestMethod]
        public async Task SelectMany_and_ToListAsync_should_return_the_Details_from_the_database()
        {
            // Act
            var detailsFromDb = await _factory.Set<OrderContext, Order>().SelectMany(o => o.Details).ToListAsync();

            // Assert
            Assert.AreEqual(100, detailsFromDb.Count);
        }

        [TestMethod]
        public void SelectMany_and_Select_and_ToList_should_return_the_Details_from_the_database()
        {
            // Act
            var detailsFromDb = _factory.Set<OrderContext, Order>().SelectMany(o => o.Details).Select(d => d.Order).ToList();

            // Assert
            Assert.AreEqual(100, detailsFromDb.Count);
        }

        [TestMethod]
        public async Task SelectMany_and_Select_and_ToListAsync_should_return_the_Details_from_the_database()
        {
            // Act
            var detailsFromDb = await _factory.Set<OrderContext, Order>().SelectMany(o => o.Details).Select(d => d.Order).ToListAsync();

            // Assert
            Assert.AreEqual(100, detailsFromDb.Count);
        }

        [TestMethod]
        public async Task Select_and_ToListAsync_should_return_the_Orders_from_the_database()
        {
            // Act
            var detailsFromDb = await _factory.Set<OrderContext, Detail>().Select(d => d.Order).ToListAsync();

            // Assert
            Assert.AreEqual(100, detailsFromDb.Count);
        }

        [TestMethod]
        public void Select_and_ToList_should_return_the_Orders_from_the_database()
        {
            // Act
            var detailsFromDb = _factory.Set<OrderContext, Detail>().Select(d => d.Order).ToList();

            // Assert
            Assert.AreEqual(100, detailsFromDb.Count);
        }

        [TestMethod]
        public async Task Select_to_anonymous_type_and_ToListAsync_should_return_the_Orders_from_the_database()
        {
            // Act
            var detailsFromDb = await _factory.Set<OrderContext, Detail>().Select(d => new { d.Id, d.Quantity }).ToListAsync();

            // Assert
            Assert.AreEqual(100, detailsFromDb.Count);
        }

        [TestMethod]
        public void Select_to_anonymous_type_and_ToList_should_return_the_Orders_from_the_database()
        {
            // Act
            var detailsFromDb = _factory.Set<OrderContext, Detail>().Select(d => new { d.Id, d.Quantity }).ToList();

            // Assert
            Assert.AreEqual(100, detailsFromDb.Count);
        }

        [TestMethod]
        public void ToList_with_Where_should_return_the_Orders_from_the_database()
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
        public void First_should_return_the_first_Order_from_the_database()
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
