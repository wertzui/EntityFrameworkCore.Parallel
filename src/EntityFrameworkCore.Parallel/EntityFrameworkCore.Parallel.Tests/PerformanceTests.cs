using EntityFrameworkCore.Parallel.DemoModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Parallel.Tests;

[TestClass]
public class PerformanceTests
{
    private static IDbContextFactory<OrderContext> _factory = default!;
    private static TimeSpan _timeSpanSerial;
    private static TimeSpan _timeSpanInclude;
    private static TimeSpan _timeSpanParallel;
    private static TimeSpan _timeSpanFactory;

    [ClassInitialize]
    public static async Task ClassInitialize(TestContext _)
    {
        // Arrange
        _factory = await TestHelper.CreateFreshDatabase(100);

        var count = 500;

        // Prime
        await GetAllWithContextSerial();
        await GetAllWithContextParallel();
        await GetAllWithFactoryParallel();

        // Act
        _timeSpanSerial = await Benchmark(GetAllWithContextSerial, count);
        _timeSpanInclude = await Benchmark(GetAllWithContextInclude, count);
        _timeSpanParallel = await Benchmark(GetAllWithContextParallel, count);
        _timeSpanFactory = await Benchmark(GetAllWithFactoryParallel, count);

        // The output will appear in the first test which is executed as "additional output".
        Console.WriteLine($" {nameof(GetAllWithContextSerial)} - {_timeSpanSerial}");
        Console.WriteLine($" {nameof(GetAllWithContextInclude)} - {_timeSpanInclude}");
        Console.WriteLine($" {nameof(GetAllWithContextParallel)} - {_timeSpanParallel}");
        Console.WriteLine($" {nameof(GetAllWithFactoryParallel)} - {_timeSpanFactory}");
    }

    [TestMethod]
    public void Parallel_should_be_faster_than_serial()
    {
        Assert.IsTrue(_timeSpanParallel < _timeSpanSerial, $"{_timeSpanParallel} was not less than {_timeSpanSerial}");
    }

    [TestMethod]
    public void Factory_should_be_faster_than_include()
    {
        Assert.IsTrue(_timeSpanFactory < _timeSpanInclude, $"{_timeSpanFactory} was not less than {_timeSpanInclude}");
    }

    [TestMethod]
    public void Factory_should_be_roughly_equal_to_parallel()
    {
        var delta = _timeSpanFactory.TotalMilliseconds * 0.1; // 10% difference is ok
        Assert.AreEqual(_timeSpanFactory.TotalMilliseconds, _timeSpanParallel.TotalMilliseconds, delta, $"{_timeSpanFactory} was not roughly equal to {_timeSpanParallel}");
    }

    private static async Task<TimeSpan> Benchmark(Func<Task> method, int count)
    {
        var sw = new Stopwatch();
        sw.Start();
        for (int i = 0; i < count; i++)
        {
            await method();
        }
        sw.Stop();
        return sw.Elapsed;
    }

    private static async Task GetAllWithContextInclude()
    {
        using var context = _factory.CreateDbContext();
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        var orders = await EntityFrameworkQueryableExtensions.ToListAsync(context.Set<Order>().Include(o => o.Details));
    }

    private static async Task GetAllWithContextSerial()
    {
        using var context = _factory.CreateDbContext();
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        var orders = await EntityFrameworkQueryableExtensions.ToListAsync(context.Set<Order>());
        var details = await EntityFrameworkQueryableExtensions.ToListAsync(context.Set<Detail>());
    }

    private static async Task GetAllWithContextParallel()
    {
        using var context1 = _factory.CreateDbContext();
        using var context2 = _factory.CreateDbContext();
        context1.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        context2.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        var ordersTask = EntityFrameworkQueryableExtensions.ToListAsync(context1.Set<Order>());
        var detailsTask = EntityFrameworkQueryableExtensions.ToListAsync(context2.Set<Detail>());

        await Task.WhenAll(ordersTask, detailsTask);

        var orders = ordersTask.Result;
        var details = detailsTask.Result;
    }

    private static async Task GetAllWithFactoryParallel()
    {
        var ordersTask = _factory.Set<OrderContext, Order>().ToListAsync();
        var detailsTask = _factory.Set<OrderContext, Detail>().ToListAsync();

        await Task.WhenAll(ordersTask, detailsTask);
        _ = ordersTask.Result;
        _ = detailsTask.Result;
    }
}
