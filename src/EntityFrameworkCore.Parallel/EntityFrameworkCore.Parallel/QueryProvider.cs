﻿using EntityFrameworkCore.Parallel.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace EntityFrameworkCore.Parallel
{
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
        /// <exception cref="System.ArgumentNullException">queryContext</exception>
        public QueryProvider(IQueryContext queryContext)
        {
            _queryContext = queryContext ?? throw new System.ArgumentNullException(nameof(queryContext));

            _genericExecuteMethod = _queryContext.GetType()
                 .GetRuntimeMethods()
                 .Single(m => m.Name == nameof(IQueryContext.Execute) && m.IsGenericMethod);
        }

        /// <inheritdoc/>
        public virtual IQueryable CreateQuery(Expression expression)
        {
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));

            var queryable = _genericCreateQueryMethod
                .MakeGenericMethod(expression.Type.GetSequenceType())
                .Invoke(this, new object[] { expression }) as IQueryable;

            if (queryable is null)
                throw new InvalidOperationException("Unable to create an IQueryable from the given expression.");

            return queryable;
        }

        /// <inheritdoc/>
        public virtual IQueryable<T> CreateQuery<T>(Expression expression)
        {
            if (expression is null)
                throw new System.ArgumentNullException(nameof(expression));

            return new EntityQueryable<T>(this, expression);
        }

        /// <inheritdoc/>
        public virtual object Execute(Expression expression)
        {
            if (expression is null)
                throw new System.ArgumentNullException(nameof(expression));

            var executeResult = _genericExecuteMethod
                .MakeGenericMethod(expression.Type)
                .Invoke(_queryContext, new object[] { expression });

            if (executeResult is null)
                throw new InvalidOperationException("The execution of the given expression resulted in a null value.");

            return executeResult;
        }

        /// <inheritdoc/>
        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            if (expression is null)
                throw new System.ArgumentNullException(nameof(expression));

            return _queryContext.Execute<TResult>(expression);
        }

        /// <inheritdoc/>
        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            if (expression is null)
                throw new System.ArgumentNullException(nameof(expression));

            return _queryContext.ExecuteAsync<TResult>(expression, cancellationToken);
        }
    }
}