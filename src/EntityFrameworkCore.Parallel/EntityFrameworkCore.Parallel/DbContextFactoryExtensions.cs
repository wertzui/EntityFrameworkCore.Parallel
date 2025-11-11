using EntityFrameworkCore.Parallel;
using EntityFrameworkCore.Parallel.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Linq;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// Contains the <see cref="Set{TEntity}(IDbContextFactory{DbContext})"/> extension method which is the starting point for any query.
/// </summary>
public static class DbContextFactoryExtensions
{
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="contextFactory">The factory which can create the <see cref="DbContext"/>.</param>
    extension<TContext>(IDbContextFactory<TContext> contextFactory) where TContext : DbContext
    {
        /// <summary>
        /// The starting point for your query if you do not want to use the <see cref="Parallel{TContext}(IDbContextFactory{TContext})"/> extension method on your context. Make sure <typeparamref name="TEntity"/> is available as a <see cref="DbSet{TEntity}"/> on your <see cref="DbContext"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entities.</typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> Set<TEntity>()
            where TEntity : class
        {
            ArgumentNullException.ThrowIfNull(contextFactory);

            var query = new EntityQueryable<TEntity>(
                new QueryProvider(new DbContextFactoryQueryContext<TContext, TEntity>(contextFactory)),
                new EntityType<TEntity>());
            return query;
        }

        /// <summary>
        /// Wraps the factory as an IDbContextFactory&lt;DbContext&gt; to allow the usage of the <see cref="Set{TEntity}(IDbContextFactory{DbContext})"/> Method without having to specify the type of the context.
        /// </summary>
        /// <returns>The factory wrapped as an IDbContextFactory&lt;DbContext&gt;.</returns>
        public IDbContextFactory<DbContext> Parallel()
            => new DbContextFactoryWrapper<TContext>(contextFactory);
    }

    /// <param name="contextFactory">The factory which can create the <see cref="DbContext"/>.</param>
    extension(IDbContextFactory<DbContext> contextFactory)
    {
        /// <summary>
        /// The starting point for your query. Make sure <typeparamref name="TEntity"/> is available as a <see cref="DbSet{TEntity}"/> on your <see cref="DbContext"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entities.</typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> Set<TEntity>()
            where TEntity : class
        {
            ArgumentNullException.ThrowIfNull(contextFactory);

            var query = new EntityQueryable<TEntity>(
                new QueryProvider(new DbContextFactoryQueryContext<DbContext, TEntity>(contextFactory)),
                new EntityType<TEntity>());
            return query;
        }
    }

    /// <typeparam name="TContextIn">The type of the context to wrap.</typeparam>
    /// <typeparam name="TContextOut">The type of the context to return.</typeparam>
    /// <param name="contextFactory">The factory which can create the <see cref="DbContext"/>.</param>
    extension<TContextIn, TContextOut>(IDbContextFactory<TContextIn> contextFactory)
        where TContextIn : TContextOut
        where TContextOut : DbContext
    {
        /// <summary>
        /// Down-casts the generic parameter <typeparamref name="TContextIn"/> of <see cref="IDbContextFactory{TContextIn}"/> to <typeparamref name="TContextOut"/>.
        /// </summary>
        /// <returns>The factory wrapped as an <see cref="IDbContextFactory{TContextOut}"/></returns>
        public IDbContextFactory<TContextOut> Cast()
            => new DbContextFactoryWrapper<TContextIn, TContextOut>(contextFactory);
    }
}
