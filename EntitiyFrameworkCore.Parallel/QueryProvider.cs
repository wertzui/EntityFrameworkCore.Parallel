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
    /// A basic query provider which will pass the execution logic down the the given <see cref="IQueryContext"/>.
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
            _queryContext = queryContext;

            _genericExecuteMethod = _queryContext.GetType()
                 .GetRuntimeMethods()
                 .Single(m => m.Name == nameof(IQueryContext.Execute) && m.IsGenericMethod);
        }

        public virtual IQueryable CreateQuery(Expression expression)
            => (IQueryable)_genericCreateQueryMethod
                .MakeGenericMethod(expression.Type.GetSequenceType())
                .Invoke(this, new object[] { expression });

        public virtual IQueryable<T> CreateQuery<T>(Expression expression)
            => new EntityQueryable<T>(this, expression);

        public virtual object Execute(Expression expression)
            => _genericExecuteMethod
            .MakeGenericMethod(expression.Type)
                .Invoke(_queryContext, new object[] { expression });

        TResult IQueryProvider.Execute<TResult>(Expression expression)
            => _queryContext.Execute<TResult>(expression);

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
            => _queryContext.ExecuteAsync<TResult>(expression, cancellationToken);
    }
}
