using EntityFrameworkCore.Parallel;
using EntityFrameworkCore.Parallel.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Linq;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// Contains the <see cref="Set{TEntity}(IDbContextFactory{DbContext})"/> extension method which is the starting point for any query.
    /// </summary>
    public static class DbContextFactoryExtensions
    {
        /// <summary>
        /// The starting point for your query. Make sure <typeparamref name="TEntity"/> is available as a <see cref="DbSet{TEntity}"/> on your <see cref="DbContext"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entities.</typeparam>
        /// <param name="contextFactory">The factory which can create the <see cref="DbContext"/>.</param>
        /// <returns></returns>
        public static IQueryable<TEntity> Set<TEntity>(this IDbContextFactory<DbContext> contextFactory)
            where TEntity : class
        {
            if (contextFactory is null)
                throw new System.ArgumentNullException(nameof(contextFactory));

            var query = new EntityQueryable<TEntity>(
                new QueryProvider(new DbContextFactoryQueryContext<TEntity>(contextFactory)),
                new EntityType<TEntity>());
            return query;
        }
    }
}
