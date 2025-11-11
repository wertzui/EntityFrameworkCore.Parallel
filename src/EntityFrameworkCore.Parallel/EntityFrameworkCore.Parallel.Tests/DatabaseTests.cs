using EntityFrameworkCore.Parallel.DemoModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Parallel.Tests;

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
        Assert.HasCount(10, ordersFromDb);
    }

    [TestMethod]
    public async Task ToListAsync_should_return_the_Orders_from_the_database()
    {
        // Act
        var ordersFromDb = await _factory.Set<OrderContext, Order>().ToListAsync(TestContext.CancellationToken);

        // Assert
        Assert.HasCount(10, ordersFromDb);
    }

    [TestMethod]
    public void SelectMany_and_ToList_should_return_the_Details_from_the_database()
    {
        // Act
        var detailsFromDb = _factory.Set<OrderContext, Order>().SelectMany(o => o.Details).ToList();

        // Assert
        Assert.HasCount(100, detailsFromDb);
    }

    [TestMethod]
    public async Task SelectMany_and_ToListAsync_should_return_the_Details_from_the_database()
    {
        // Act
        var detailsFromDb = await _factory.Set<OrderContext, Order>().SelectMany(o => o.Details).ToListAsync(TestContext.CancellationToken);

        // Assert
        Assert.HasCount(100, detailsFromDb);
    }

    [TestMethod]
    public void SelectMany_and_Select_and_ToList_should_return_the_Details_from_the_database()
    {
        // Act
        var detailsFromDb = _factory.Set<OrderContext, Order>().SelectMany(o => o.Details).Select(d => d.Order).ToList();

        // Assert
        Assert.HasCount(100, detailsFromDb);
    }

    [TestMethod]
    public async Task SelectMany_and_Select_and_ToListAsync_should_return_the_Details_from_the_database()
    {
        // Act
        var detailsFromDb = await _factory.Set<OrderContext, Order>().SelectMany(o => o.Details).Select(d => d.Order).ToListAsync(TestContext.CancellationToken);

        // Assert
        Assert.HasCount(100, detailsFromDb);
    }

    [TestMethod]
    public async Task Select_and_ToListAsync_should_return_the_Orders_from_the_database()
    {
        // Act
        var detailsFromDb = await _factory.Set<OrderContext, Detail>().Select(d => d.Order).ToListAsync(TestContext.CancellationToken);

        // Assert
        Assert.HasCount(100, detailsFromDb);
    }

    [TestMethod]
    public void Select_and_ToList_should_return_the_Orders_from_the_database()
    {
        // Act
        var detailsFromDb = _factory.Set<OrderContext, Detail>().Select(d => d.Order).ToList();

        // Assert
        Assert.HasCount(100, detailsFromDb);
    }

    [TestMethod]
    public async Task Select_to_anonymous_type_and_ToListAsync_should_return_the_Orders_from_the_database()
    {
        // Act
        var detailsFromDb = await _factory.Set<OrderContext, Detail>().Select(d => new { d.Id, d.Quantity }).ToListAsync(TestContext.CancellationToken);

        // Assert
        Assert.HasCount(100, detailsFromDb);
    }

    [TestMethod]
    public void Select_to_anonymous_type_and_ToList_should_return_the_Orders_from_the_database()
    {
        // Act
        var detailsFromDb = _factory.Set<OrderContext, Detail>().Select(d => new { d.Id, d.Quantity }).ToList();

        // Assert
        Assert.HasCount(100, detailsFromDb);
    }

    [TestMethod]
    public void ToList_with_Where_should_return_the_Orders_from_the_database()
    {
        // Act
        var ordersFromDb = _factory.Set<OrderContext, Order>().Where(o => o.Id > 3).ToList();

        // Assert
        Assert.HasCount(7, ordersFromDb);
    }

    [TestMethod]
    public async Task ToListAsync_with_Where_should_return_the_Orders_from_the_database()
    {
        // Act
        var ordersFromDb = await _factory.Set<OrderContext, Order>().Where(o => o.Id > 3).ToListAsync(TestContext.CancellationToken);

        // Assert
        Assert.HasCount(7, ordersFromDb);
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
    public void FirstOrDefault_should_return_the_first_Order_from_the_database()
    {
        // Act
        var orderFromDb = _factory.Set<OrderContext, Order>().FirstOrDefault();

        // Assert
        Assert.IsNotNull(orderFromDb);
    }

    [TestMethod]
    public async Task FirstAsync_should_return_the_first_Order_from_the_database()
    {
        // Act
        var orderFromDb = await _factory.Set<OrderContext, Order>().FirstAsync(TestContext.CancellationToken);

        // Assert
        Assert.IsNotNull(orderFromDb);
    }

    [TestMethod]
    public void Select_to_anonymous_type_and_FirstOrDefault_should_return_the_first_Order_from_the_database()
    {
        // Act
        var orderFromDb = _factory.Set<OrderContext, Order>().Select(o => new { o.Id, o.OrderNumber }).FirstOrDefault();

        // Assert
        Assert.IsNotNull(orderFromDb);
    }

    [TestMethod]
    public async Task FirstOrDefaultAsync_should_return_the_first_Order_from_the_database()
    {
        // Act
        var orderFromDb = await _factory.Set<OrderContext, Order>().FirstOrDefaultAsync(TestContext.CancellationToken);

        // Assert
        Assert.IsNotNull(orderFromDb);
    }

    [TestMethod]
    public async Task Select_to_anonymous_type_and_FirstOrDefaultAsync_should_return_the_first_Order_from_the_database()
    {
        // Act
        var orderFromDb = await _factory.Set<OrderContext, Order>().Select(o => new { o.Id, o.OrderNumber }).FirstOrDefaultAsync(TestContext.CancellationToken);

        // Assert
        Assert.IsNotNull(orderFromDb);
    }


    [TestMethod]
    public async Task SingleAsync_should_return_the_Single_Order_from_the_database()
    {
        // Act
        var orderFromDb = await _factory.Set<OrderContext, Order>().Where(o => o.Id == 1).SingleAsync(TestContext.CancellationToken);

        // Assert
        Assert.IsNotNull(orderFromDb);
    }

    [TestMethod]
    public void Select_to_anonymous_type_and_SingleOrDefault_should_return_the_Single_Order_from_the_database()
    {
        // Act
        var orderFromDb = _factory.Set<OrderContext, Order>().Where(o => o.Id == 1).Select(o => new { o.Id, o.OrderNumber }).SingleOrDefault();

        // Assert
        Assert.IsNotNull(orderFromDb);
    }

    [TestMethod]
    public async Task SingleOrDefaultAsync_should_return_the_Single_Order_from_the_database()
    {
        // Act
        var orderFromDb = await _factory.Set<OrderContext, Order>().Where(o => o.Id == 1).SingleOrDefaultAsync(TestContext.CancellationToken);

        // Assert
        Assert.IsNotNull(orderFromDb);
    }

    [TestMethod]
    public async Task Select_to_anonymous_type_and_SingleOrDefaultAsync_should_return_the_Single_Order_from_the_database()
    {
        // Act
        var orderFromDb = await _factory.Set<OrderContext, Order>().Where(o => o.Id == 1).Select(o => new { o.Id, o.OrderNumber }).SingleOrDefaultAsync(TestContext.CancellationToken);

        // Assert
        Assert.IsNotNull(orderFromDb);
    }

    public TestContext TestContext { get; set; }
}
