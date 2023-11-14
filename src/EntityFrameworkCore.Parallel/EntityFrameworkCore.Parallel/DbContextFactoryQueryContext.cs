using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Parallel;

/// <summary>
/// This class contains the logic which will actually create the <see cref="DbContext"/> and
/// execute the query on it.
/// </summary>
/// <typeparam name="TContext">The type of the context.</typeparam>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public class DbContextFactoryQueryContext<TContext, TEntity> : DbContextFactoryQueryContext, IQueryContext
    where TContext : DbContext
    where TEntity : class
{
    private readonly IDbContextFactory<TContext> factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="DbContextFactoryQueryContext{TEntity}"/> class.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <exception cref="ArgumentNullException">factory</exception>
    public DbContextFactoryQueryContext(IDbContextFactory<TContext> factory)
    {
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    /// <inheritdoc/>
    public TResult Execute<TResult>(Expression query)
    {
        ArgumentNullException.ThrowIfNull(query);

        var context = factory.CreateDbContext();
        try
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var set = context.Set<TEntity>();
            var queryable = set.AsQueryable();
            var provider = queryable.Provider;

            var replaced = ReplaceProvider(query, set, provider);
            var result = provider.Execute<TResult>(replaced);
            var autoDisposing = DisposeAfterEnumeration(result, context);
            return autoDisposing;
        }
        catch
        {
            context.Dispose();
            throw;
        }
    }

    /// <inheritdoc/>
    public TResult ExecuteAsync<TResult>(Expression query, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        // We cannot use a using block here, because the result will be enumerated after this
        // method has already returned. Instead we pass the DbContext down to the enumerator
        // which will then dispose the context once itself gets disposed. This will happen when
        // the result is enumerated.
        var context = factory.CreateDbContext();
        try
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var set = context.Set<TEntity>();
            var queryable = set.AsQueryable();
            var provider = queryable.Provider;

            if (provider is IAsyncQueryProvider asyncProvider)
            {
                var replaced = ReplaceProvider(query, set, provider);
                var result = asyncProvider.ExecuteAsync<TResult>(replaced, cancellationToken);
                var autoDisposing = DisposeAfterEnumerationAsync(result, context);
                return autoDisposing;
            }

            throw new InvalidOperationException("Cannot execute an async query on a non async query provider.");
        }
        catch
        {
            context.Dispose();
            throw;
        }
    }

    private static TResult DisposeAfterEnumeration<TResult>(TResult result, DbContext dbContext)
    {
        if (result is IEnumerable enumerableResult)
        {
            var autoDisposing = MakeAutoDisposing<TResult>(enumerableResult, dbContext);
            return autoDisposing;
        }

        dbContext.Dispose();
        return result;
    }

    private static TResult DisposeAfterEnumerationAsync<TResult>(TResult result, DbContext dbContext)
    {
        if (result is IEnumerable enumerableResult)
        {
            var autoDisposing = MakeAsyncAutoDisposing<TResult>(enumerableResult, dbContext);
            return autoDisposing;
        }

        if (result is Task taskResult)
        {
            var autoDisposing = MakeAsyncAutoDisposing<TResult>(taskResult, dbContext);
            return autoDisposing;
        }

        dbContext.Dispose();
        return result;
    }

    private static Expression ReplaceProvider(Expression query, DbSet<TEntity> set, IQueryProvider provider)
                                        => QueryRootExpressionReplaceVisitor.ReplaceProvider(query, set, provider);
}

/// <summary>
/// Base class for <see cref="DbContextFactoryQueryContext{TEntity}"/> which contains some non
/// generic reflection stuff.
/// </summary>
/// <seealso cref="EntityFrameworkCore.Parallel.DbContextFactoryQueryContext"/>
/// <seealso cref="EntityFrameworkCore.Parallel.IQueryContext"/>
public abstract class DbContextFactoryQueryContext
{
    /// <summary>
    /// <see cref="MethodInfo"/> of <see cref="DisposeEnumerableAfterEnumerationAsync{TResultElement}(IAsyncEnumerable{TResultElement}, DbContext)"/>.
    /// </summary>
    private static readonly MethodInfo _disposeEnumerableAfterEnumerationAsyncMethodInfo = typeof(DbContextFactoryQueryContext)
        .GetMethod(nameof(DisposeEnumerableAfterEnumerationAsync), BindingFlags.Static | BindingFlags.NonPublic)
        ?? throw new MissingMethodException(nameof(DbContextFactoryQueryContext), nameof(DisposeEnumerableAfterEnumerationAsync));

    /// <summary>
    /// <see cref="MethodInfo"/> of <see cref="DisposeEnumerableAfterEnumeration{TResultElement}(IEnumerable{TResultElement}, DbContext)"/>.
    /// </summary>
    private static readonly MethodInfo _disposeEnumerableAfterEnumerationMethodInfo = typeof(DbContextFactoryQueryContext)
        .GetMethod(nameof(DisposeEnumerableAfterEnumeration), BindingFlags.Static | BindingFlags.NonPublic)
        ?? throw new MissingMethodException(nameof(DbContextFactoryQueryContext), nameof(DisposeEnumerableAfterEnumeration));

    /// <summary>
    /// <see cref="MethodInfo"/> of <see cref="DisposeAfterAwait{TResult}(Task{TResult}, DbContext)"/>.
    /// </summary>
    private static readonly MethodInfo _disposeAfterAwaitMethodInfo = typeof(DbContextFactoryQueryContext)
        .GetMethod(nameof(DisposeAfterAwait), BindingFlags.Static | BindingFlags.NonPublic)
        ?? throw new MissingMethodException(nameof(DbContextFactoryQueryContext), nameof(DisposeAfterAwait));

    /// <summary>
    /// <see cref="MethodInfo"/> of <see cref="AsyncEnumerable.ToAsyncEnumerable{TSource}(Task{TSource})"/>.
    /// </summary>
    protected static readonly MethodInfo _toAsyncEnumerableMethodInfo = typeof(AsyncEnumerable)
        .GetMethods()
        .Single(m =>
        {
            if (m.Name != nameof(AsyncEnumerable.ToAsyncEnumerable))
                return false;

            var parameter = m.GetParameters().FirstOrDefault();
            if (parameter == null)
                return false;

            var parameterType = parameter.ParameterType;
            if (!typeof(Task).IsAssignableFrom(parameterType))
                return false;

            return true;
        });

    /// <summary>
    /// <see cref="MethodInfo"/> of <see cref="AsyncEnumerable.ToListAsync{TSource}(IAsyncEnumerable{TSource}, CancellationToken)"/>.
    /// </summary>
    protected static readonly MethodInfo _toListAsyncMethodInfo = typeof(AsyncEnumerable)
        ?.GetMethod(nameof(AsyncEnumerable.ToListAsync)) ?? throw new MissingMethodException(nameof(AsyncEnumerable), nameof(AsyncEnumerable.ToListAsync));

    /// <summary>
    /// <see cref="MethodInfo"/> of <see cref="Enumerable.ToList{TSource}(IEnumerable{TSource})"/>.
    /// </summary>
    protected static readonly MethodInfo _toListMethodInfo = typeof(Enumerable)
        ?.GetMethod(nameof(Enumerable.ToList)) ?? throw new MissingMethodException(nameof(Enumerable), nameof(Enumerable.ToList));

    /// <summary>
    /// Wraps the <paramref name="result"/> in an <see cref="AutoDisposingEnumerable{T}"/> which disposes the <paramref name="dbContext"/> after enumeration.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="result">The result.</param>
    /// <param name="dbContext">The DB context.</param>
    protected static TResult MakeAutoDisposing<TResult>(IEnumerable result, DbContext dbContext)
    {
        var resultType = typeof(TResult);
        var elementType = resultType.IsGenericType ? resultType.GetGenericArguments()[0] : resultType.GetElementType() ?? typeof(object);
        var genericMethod = _disposeEnumerableAfterEnumerationMethodInfo.MakeGenericMethod(elementType);
        var autoDisposing = genericMethod.Invoke(null, new object[] { result, dbContext })!;

        return (TResult)autoDisposing;
    }

    /// <summary>
    /// Wraps the <paramref name="result"/> in an <see cref="AutoDisposingAsyncEnumerable{T}"/> which disposes the <paramref name="dbContext"/> after enumeration.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="result">The result.</param>
    /// <param name="dbContext">The DB context.</param>
    protected static TResult MakeAsyncAutoDisposing<TResult>(IEnumerable result, DbContext dbContext)
    {
        var resultType = typeof(TResult);
        var elementType = resultType.IsGenericType ? resultType.GetGenericArguments()[0] : resultType.GetElementType() ?? typeof(object);
        var genericMethod = _disposeEnumerableAfterEnumerationAsyncMethodInfo.MakeGenericMethod(elementType);
        var autoDisposing = genericMethod.Invoke(null, new object[] { result, dbContext })!;

        return (TResult)autoDisposing;
    }


    /// <summary>
    /// Wraps the <paramref name="result"/> in a <see cref="Task"/> which disposes the <paramref name="dbContext"/> after awaiting the original <see cref="Task"/> and returns the original <see cref="Task"/> as <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="result">The result.</param>
    /// <param name="dbContext">The DB context.</param>
    protected static TResult MakeAsyncAutoDisposing<TResult>(Task result, DbContext dbContext)
    {
        var resultType = typeof(TResult);
        var elementType = resultType.IsGenericType ? resultType.GetGenericArguments()[0] : typeof(object);
        var genericMethod = _disposeAfterAwaitMethodInfo.MakeGenericMethod(elementType);
        var autoDisposing = genericMethod.Invoke(null, new object[] { result, dbContext })!;

        return (TResult)autoDisposing;
    }

    private static IEnumerable<TResultElement> DisposeEnumerableAfterEnumeration<TResultElement>(IEnumerable<TResultElement> result, DbContext dbContext)
    {
        var autoDisposing = EnumerableExtensions.DisposeAfterEnumeration(result, dbContext);
        return autoDisposing;
    }

    private static IAsyncEnumerable<TResultElement> DisposeEnumerableAfterEnumerationAsync<TResultElement>(IAsyncEnumerable<TResultElement> result, DbContext dbContext)
    {
        var autoDisposing = AsyncEnumerableExtensions.DisposeAfterEnumeration(result, dbContext);
        return autoDisposing;
    }

    private async static Task<TResultElement> DisposeAfterAwait<TResultElement>(Task<TResultElement> task, DbContext dbContext)
    {
        var result = await task;
        await dbContext.DisposeAsync();
        return result;
    }
}