using EntityFrameworkCore.Parallel.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace EntityFrameworkCore.Parallel;

/// <summary>
/// A basic query provider which will pass the execution logic down the given <see cref="IQueryContext"/>.
/// </summary>
public class QueryProvider : IAsyncQueryProvider
{
    private readonly IQueryContext _queryContext;

    private static readonly MethodInfo _genericCreateQueryMethod
        = typeof(QueryProvider)
            .GetRuntimeMethods()
            .Single(m => m.Name == nameof(CreateQuery) && m.IsGenericMethod);

    private readonly MethodInfo _genericExecuteMethod;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryProvider"/> class.
    /// </summary>
    /// <param name="queryContext">The query context.</param>
    /// <exception cref="ArgumentNullException">queryContext</exception>
    public QueryProvider(IQueryContext queryContext)
    {
        _queryContext = queryContext ?? throw new ArgumentNullException(nameof(queryContext));

        _genericExecuteMethod = _queryContext.GetType()
             .GetRuntimeMethods()
             .Single(m => m.Name == nameof(IQueryContext.Execute) && m.IsGenericMethod);
    }

    /// <inheritdoc/>
    public virtual IQueryable CreateQuery(Expression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        if (_genericCreateQueryMethod
            .MakeGenericMethod(expression.Type.GetSequenceType())
            .Invoke(this, [expression]) is not IQueryable queryable)
            throw new InvalidOperationException("Unable to create an IQueryable from the given expression.");

        return queryable;
    }

    /// <inheritdoc/>
    public virtual IQueryable<T> CreateQuery<T>(Expression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        return new EntityQueryable<T>(this, expression);
    }

    /// <inheritdoc/>
    public virtual object Execute(Expression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        var executeResult = _genericExecuteMethod
            .MakeGenericMethod(expression.Type)
            .Invoke(_queryContext, [expression]) ?? throw new InvalidOperationException("The execution of the given expression resulted in a null value.");

        return executeResult;
    }

    /// <inheritdoc/>
    TResult IQueryProvider.Execute<TResult>(Expression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        return _queryContext.Execute<TResult>(expression);
    }

    /// <inheritdoc/>
    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);

        return _queryContext.ExecuteAsync<TResult>(expression, cancellationToken);
    }
}