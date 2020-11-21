using EntitiyFrameworkCore.Parallel.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace EntitiyFrameworkCore.Parallel
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

        public QueryProvider(IQueryContext queryContext)
        {
            _queryContext = queryContext ?? throw new System.ArgumentNullException(nameof(queryContext));

            _genericExecuteMethod = _queryContext.GetType()
                 .GetRuntimeMethods()
                 .Single(m => m.Name == nameof(IQueryContext.Execute) && m.IsGenericMethod);
        }

        public virtual IQueryable CreateQuery(Expression expression)
        {
            if (expression is null)
                throw new System.ArgumentNullException(nameof(expression));

            return (IQueryable)_genericCreateQueryMethod
                .MakeGenericMethod(expression.Type.GetSequenceType())
                .Invoke(this, new object[] { expression });
        }

        public virtual IQueryable<T> CreateQuery<T>(Expression expression)
        {
            if (expression is null)
                throw new System.ArgumentNullException(nameof(expression));

            return new EntityQueryable<T>(this, expression);
        }

        public virtual object Execute(Expression expression)
        {
            if (expression is null)
                throw new System.ArgumentNullException(nameof(expression));

            return _genericExecuteMethod
            .MakeGenericMethod(expression.Type)
                .Invoke(_queryContext, new object[] { expression });
        }

        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            if (expression is null)
                throw new System.ArgumentNullException(nameof(expression));

            return _queryContext.Execute<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            if (expression is null)
                throw new System.ArgumentNullException(nameof(expression));

            return _queryContext.ExecuteAsync<TResult>(expression, cancellationToken);
        }
    }
}