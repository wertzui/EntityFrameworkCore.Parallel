using EntityFrameworkCore.Parallel.DemoModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Parallel.Tests
{
    [TestClass]
    public class CastExtensionTests
    {
        private IDbContextFactory<OrderContext> _factory = default!;

        [TestInitialize]
        public async Task TestInitialize()
        {
            _factory = await TestHelper.CreateFreshDatabase(10);
        }

        [TestMethod]
        public void WithParallel_and_Set_of_TEntity_should_return_the_same_value_as_Set_of_TContext_TEntity()
        {
            // Act
            var orderWithParallelAndSet = _factory.Parallel().Set<Order>().First();
            var orderWithSetOnly = _factory.Set<OrderContext, Order>().First();

            // Assert
            Assert.AreEqual(orderWithSetOnly.Id, orderWithParallelAndSet.Id);
        }
    }
}
