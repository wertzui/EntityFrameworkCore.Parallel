using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Parallel
{
    /// <summary>
    /// This class wraps an <see cref="IDbContextFactory{TContext}"/> so it can be used with the <see cref="DbContextFactoryExtensions.Set{TEntity}(IDbContextFactory{DbContext})"/> extension method.
    /// This became necessary because in Entity Framework Core 6 the <see cref="IDbContextFactory{TContext}"/> interface is no longer covariant.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <remarks>See https://github.com/dotnet/efcore/issues/26630</remarks>
    public class DbContextFactoryWrapper<TContext> : IDbContextFactory<DbContext>
        where TContext : DbContext
    {
        private readonly IDbContextFactory<TContext> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextFactoryWrapper{TContext}"/> class.
        /// </summary>
        /// <param name="factory">The factory to wrap.</param>
        public DbContextFactoryWrapper(IDbContextFactory<TContext> factory)
        {
            _factory = factory;
        }

        /// <inheritdoc/>
        public DbContext CreateDbContext() => _factory.CreateDbContext();
    }
}
