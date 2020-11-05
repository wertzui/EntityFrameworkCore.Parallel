using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace EntitiyFrameworkCore.Parallel
{
    /// <summary>
    /// This class contains the logic which will actually create the <see cref="DbContext"/> and execute the query on it.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public class DbContextFactoryQueryContext<TEntity> : DbContextFactoryQueryContext, IQueryContext
        //where TContext : DbContext
        where TEntity : class
    {
        private readonly IDbContextFactory<DbContext> factory;

        public DbContextFactoryQueryContext(IDbContextFactory<DbContext> factory)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public TResult Execute<TResult>(Expression query)
        {
            using (var context = factory.CreateDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                var set = context.Set<TEntity>();
                var queryaple = set.AsQueryable();
                var provider = queryaple.Provider;

                var replaced = ReplaceProvider(query, set, provider);
                var result = provider.Execute<TResult>(replaced);
                var buffered = Buffer(result);
                return buffered;
            }
        }

        private static Expression ReplaceProvider(Expression query, DbSet<TEntity> set, IQueryProvider provider)
        {
            var setQuery = provider is IAsyncQueryProvider asyncProvider ?
                                new QueryRootExpression(asyncProvider, set.EntityType) :
                                new QueryRootExpression(set.EntityType);
            Expression replaced;
            if (query is MethodCallExpression call)
                replaced = call.Update(call.Object, new Expression[] { setQuery }.Concat(call.Arguments.Skip(1)));
            else if (query is QueryRootExpression root)
                replaced = setQuery;
            else
                throw new ArgumentException($"An Expression of type {query.Type} is not supported.", nameof(query));
            return replaced;
        }

        private static TResult Buffer<TResult>(TResult result, CancellationToken cancellationToken = default)
        {
            if (result is IEnumerable enumerable)
            {
                var type = enumerable.GetType();
                if (type.GenericTypeArguments.Length == 1)
                {
                    var genericArgument = type.GenericTypeArguments[0];

                    var genericToList = _toListMethodInfo.MakeGenericMethod(genericArgument);
                    var list = genericToList.Invoke(null, new object[] { enumerable });

                    var casted = (TResult)list;
                    return casted;
                }
            }

            return result;
        }

        public TResult ExecuteAsync<TResult>(Expression query, CancellationToken cancellationToken)
        {
            // We cannot use a using block here, because the result will be enumerated after this method has already returned.
            // Instead we pass the DbContext down to the enumerator which will then dispose the context once itself gets disposed.
            // This will happen when the result is enumerated.
            var context = factory.CreateDbContext();
            try
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                var set = context.Set<TEntity>();
                var queryaple = set.AsQueryable();
                var provider = queryaple.Provider;

                if (provider is IAsyncQueryProvider asyncProvider)
                {
                    var replaced = ReplaceProvider(query, set, provider);
                    var result = asyncProvider.ExecuteAsync<TResult>(replaced);
                    var buffered = BufferAsync(result, context, cancellationToken);
                    return buffered;
                }

                throw new InvalidOperationException("Cannot execute an async query on a non async query provider.");
            }
            catch
            {
                context.Dispose();
                throw;
            }
        }

        private static TResult BufferAsync<TResult>(TResult result, IAsyncDisposable dbSet, CancellationToken cancellationToken)
        {
            if (result is IEnumerable enumerable)
            {
                var buffered = EntitiyFrameworkCore.Parallel.AsyncEnumerableExtensions.Buffer((dynamic)result, dbSet);
                return (TResult)buffered;
            }

            return result;
        }
    }


    public abstract class DbContextFactoryQueryContext
    {
        protected static readonly MethodInfo _toListMethodInfo = typeof(Enumerable)
                        .GetMethod(nameof(Enumerable.ToList));

        protected static readonly MethodInfo _toListAsyncMethodInfo = typeof(AsyncEnumerable)
                        .GetMethod(nameof(AsyncEnumerable.ToListAsync));

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
    }
}
