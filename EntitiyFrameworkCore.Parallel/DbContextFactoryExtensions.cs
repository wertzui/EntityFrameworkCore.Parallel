using EntitiyFrameworkCore.Parallel;
using EntitiyFrameworkCore.Parallel.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Linq;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// Contains the <see cref="Set{TContext, TEntity}(IDbContextFactory{TContext})"/> extension method wich is the starting point for any query.
    /// </summary>
    public static class DbContextFactoryExtensions
    {
        /// <summary>
        /// The starting point for your query. Make sure <typeparamref name="TEntity"/> is available as a <see cref="DbSet{TEntity}"/> on your <typeparamref name="TContext"/>.
        /// </summary>
        /// <typeparam name="TContext">The type of the <see cref="DbContext"/>.</typeparam>
        /// <typeparam name="TEntity">The type fo the entities.</typeparam>
        /// <param name="contextFactory">The factory which can create the <see cref="DbContext"/>.</param>
        /// <returns></returns>
        public static IQueryable<TEntity> Set<TContext, TEntity>(this IDbContextFactory<TContext> contextFactory)
            where TContext : DbContext
            where TEntity : class
        {
            var query = new EntityQueryable<TEntity>(
                new QueryProvider(new DbContextFactoryQueryContext<TContext, TEntity>(contextFactory)),
                new EntityType<TEntity>());
            return query;
        }
    }
}
