using EntityFrameworkCore.Parallel;
using EntityFrameworkCore.Parallel.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Linq;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// Contains the <see cref="Set{TEntity}(IDbContextFactory{DbContext})"/> extension method which is the starting point for any query.
/// </summary>
public static class DbContextFactoryExtensions
{
    /// <summary>
    /// The starting point for your query if you do not want to use the <see cref="Parallel{TContext}(IDbContextFactory{TContext})"/> extension method on your context. Make sure <typeparamref name="TEntity"/> is available as a <see cref="DbSet{TEntity}"/> on your <see cref="DbContext"/>.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TEntity">The type of the entities.</typeparam>
    /// <param name="contextFactory">The factory which can create the <see cref="DbContext"/>.</param>
    /// <returns></returns>
    public static IQueryable<TEntity> Set<TContext, TEntity>(this IDbContextFactory<TContext> contextFactory)
        where TContext : DbContext
        where TEntity : class
    {
        System.ArgumentNullException.ThrowIfNull(contextFactory);

        var query = new EntityQueryable<TEntity>(
            new QueryProvider(new DbContextFactoryQueryContext<TContext, TEntity>(contextFactory)),
            new EntityType<TEntity>());
        return query;
    }
    /// <summary>
    /// The starting point for your query. Make sure <typeparamref name="TEntity"/> is available as a <see cref="DbSet{TEntity}"/> on your <see cref="DbContext"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entities.</typeparam>
    /// <param name="contextFactory">The factory which can create the <see cref="DbContext"/>.</param>
    /// <returns></returns>
    public static IQueryable<TEntity> Set<TEntity>(this IDbContextFactory<DbContext> contextFactory)
        where TEntity : class
    {
        System.ArgumentNullException.ThrowIfNull(contextFactory);

        var query = new EntityQueryable<TEntity>(
            new QueryProvider(new DbContextFactoryQueryContext<DbContext, TEntity>(contextFactory)),
            new EntityType<TEntity>());
        return query;
    }

    /// <summary>
    /// Wraps the factory as an IDbContextFactory&lt;DbContext&gt; to allow the usage of the <see cref="Set{TEntity}(IDbContextFactory{DbContext})"/> Method without having to specify the type of the context.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="contextFactory">The factory which can create the <see cref="DbContext"/>.</param>
    /// <returns>The factory wrapped as an IDbContextFactory&lt;DbContext&gt;.</returns>
    public static IDbContextFactory<DbContext> Parallel<TContext>(this IDbContextFactory<TContext> contextFactory)
        where TContext : DbContext
        => new DbContextFactoryWrapper<TContext>(contextFactory);

    /// <summary>
    /// Down-casts the generic parameter <typeparamref name="TContextIn"/> of <see cref="IDbContextFactory{TContextIn}"/> to <typeparamref name="TContextOut"/>.
    /// </summary>
    /// <typeparam name="TContextIn">The type of the context to wrap.</typeparam>
    /// <typeparam name="TContextOut">The type of the context to return.</typeparam>
    /// <param name="contextFactory">The factory which can create the <see cref="DbContext"/>.</param>
    /// <returns>The factory wrapped as an <see cref="IDbContextFactory{TContextOut}"/></returns>
    public static IDbContextFactory<TContextOut> Cast<TContextIn, TContextOut>(this IDbContextFactory<TContextIn> contextFactory)
        where TContextIn : TContextOut
        where TContextOut : DbContext
        => new DbContextFactoryWrapper<TContextIn, TContextOut>(contextFactory);
}
