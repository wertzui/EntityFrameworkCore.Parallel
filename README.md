# EntityFrameworkCore.Parallel

This extension to Entity Framework Core adds a `Set<TContext, TEntity>()` method to `IDbContextFactory<TContext>`.
For easy use it also adds a `Parallel()` method to `IDbContextFactory<TContext>` and a `Set<TEntity>()` method to `IDbContextFactory<DbContext>`.
This allows you to easily execute multiple queries in parallel without the need to write complex code, or a lot of `using` blocks or statements.
You can stick to all your known methods from `IQueryable<TEntity>`.
As the context is disposed after your query is executed, all results will obviously not be tracked and disconnected from any `DbContext`.

# How to use

In your `Program.cs`, add a `DbContextfactory`. If you want a pooled one, or not is up to you.

```csharp
builder.Services.AddPooledDbContextFactory<OrderContext>(options => options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=OrderTest"));
```

In your business class, inject an `IDbContextFactory<TContext>` and use it like this

```csharp
var ordersFromDb = await _factory.Parallel().Set<Order>().Where(o => o.Id > 3).ToListAsync(cancellationToken);
```

Or like this

```csharp
var ordersFromDb = await _factory.Set<MyContext, Order>().Where(o => o.Id > 3).ToListAsync(cancellationToken);
```

# Migration from Entity Framework Core 5 to 6

Since Entity Framework Core 6, `IDbContextFactory<TContext>` is no longer covariant (`TContext` is not marked with the `out` keyword).
Because of that, the following code does no longer work.

```csharp
var ordersFromDb = await _factory.Set<Order>().Where(o => o.Id > 3).ToListAsync();
```

See the *How to use* section for two possibilities that you can use instead.
If you want some background information, have a look at <https://github.com/dotnet/efcore/issues/26630>