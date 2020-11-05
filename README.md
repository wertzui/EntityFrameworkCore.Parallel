# EntityFrameworkCore.Parallel
This extension to Entity Framework Core adds a `Set<TContext, TEntity>` method to `IDbCOntextFactory<TContext>`.
This allows you to easily execute multiple queries in parallel without the need to write complex code, or a lot of `using` blocks or statements.
You can stick to all your known methods from `IQueryable<TEntity>`.
As the context is disposed after your query is executed, all results will obviously not be tracked and disconnected from any `DbContext`.

# How to use
In your `Startup` class, add a `DbContextfactory`. If you want a pooled one, or not is up to you.
```
services.AddPooledDbContextFactory<OrderContext>(options => options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=OrderTest"));
```

In your business class, inject an `IDbContextFactory<TContext>` and use it like this
```
var ordersFromDb = await _factory.Set<OrderContext, Order>().Where(o => o.Id > 3).ToListAsync();
```