using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Parallel
{
    /// <summary>
    /// This class wraps an <see cref="IDbContextFactory{TContextIn}"/> as <see cref="IDbContextFactory{TContextOut}"/>.
    /// It is basically down-casting <typeparamref name="TContextIn"/> to <typeparamref name="TContextOut"/>.
    /// This became necessary because in Entity Framework Core 6 the <see cref="IDbContextFactory{TContext}"/> interface is no longer covariant.
    /// </summary>
    /// <typeparam name="TContextIn">The type of the context to wrap.</typeparam>
    /// <typeparam name="TContextOut">The type of the context to return.</typeparam>
    /// <remarks>See https://github.com/dotnet/efcore/issues/26630</remarks>
    public class DbContextFactoryWrapper<TContextIn, TContextOut> : IDbContextFactory<TContextOut>
        where TContextIn : TContextOut
        where TContextOut : DbContext
    {
        private readonly IDbContextFactory<TContextIn> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextFactoryWrapper{TContextIn, TContextOut}"/> class.
        /// </summary>
        /// <param name="factory">The factory to wrap.</param>
        public DbContextFactoryWrapper(IDbContextFactory<TContextIn> factory)
        {
            _factory = factory ?? throw new System.ArgumentNullException(nameof(factory));
        }

        /// <inheritdoc/>
        public TContextOut CreateDbContext() => _factory.CreateDbContext();
    }

    /// <summary>
    /// This class wraps an <see cref="IDbContextFactory{TContext}"/> so it can be used with the <see cref="DbContextFactoryExtensions.Set{TEntity}(IDbContextFactory{DbContext})"/> extension method.
    /// This became necessary because in Entity Framework Core 6 the <see cref="IDbContextFactory{TContext}"/> interface is no longer covariant.
    /// </summary>
    /// <typeparam name="TContext">The type of the context to wrap.</typeparam>
    /// <remarks>See https://github.com/dotnet/efcore/issues/26630</remarks>
    public class DbContextFactoryWrapper<TContext> : DbContextFactoryWrapper<TContext, DbContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextFactoryWrapper{TContext}"/> class.
        /// </summary>
        /// <param name="factory">The factory to wrap.</param>
        public DbContextFactoryWrapper(IDbContextFactory<TContext> factory)
            : base(factory)
        {
        }
    }
}
