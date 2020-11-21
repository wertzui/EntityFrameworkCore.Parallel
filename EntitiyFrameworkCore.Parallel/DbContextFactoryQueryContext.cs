﻿using Microsoft.EntityFrameworkCore;
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
            if (query is null)
                throw new ArgumentNullException(nameof(query));

            var context = factory.CreateDbContext();
            try
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                var set = context.Set<TEntity>();
                var queryaple = set.AsQueryable();
                var provider = queryaple.Provider;

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

        private static Expression ReplaceProvider(Expression query, DbSet<TEntity> set, IQueryProvider provider)
            => QueryRootExpressionReplaceVisitor.ReplaceProvider(query, set, provider);

        private static TResult DisposeAfterEnumeration<TResult>(TResult result, DbContext dbContext)
        {
            if (result is IEnumerable)
            {
                var autoDisposing = EnumerableExtensions.DisposeAfterEnumeration((dynamic)result, dbContext);
                return (TResult)autoDisposing;
            }

            dbContext.Dispose();
            return result;
        }

        public TResult ExecuteAsync<TResult>(Expression query, CancellationToken cancellationToken = default)
        {
            if (query is null)
                throw new ArgumentNullException(nameof(query));

            // We cannot use a using block here, because the result will be enumerated after this method has already returned.
            // Instead we pass the DbContext down to the enumerator which will then dispose the context once itself gets disposed.
            // This will happen when the result is enumerated.
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
                    var autoDisposing = DisposeAfterEnumerationAsync(result, context, cancellationToken);
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

        private static TResult DisposeAfterEnumerationAsync<TResult>(TResult result, DbContext dbContext, CancellationToken cancellationToken = default)
        {
            if (result is IEnumerable)
            {
                var autoDisposing = AsyncEnumerableExtensions.DisposeAfterEnumeration((dynamic)result, dbContext);
                return (TResult)autoDisposing;
            }

            if (result is Task)
            {
                var autoDisposing = AutoDisposeTask((dynamic)result, dbContext, cancellationToken);
                return (TResult)autoDisposing;
            }

            dbContext.Dispose();
            return result;
        }

        private static Task<T> AutoDisposeTask<T>(Task<T> task, IAsyncDisposable asyncDisposable, CancellationToken cancellationToken = default)
            => task
                .ContinueWith(async t => { await asyncDisposable.DisposeAsync(); return t.Result; }, cancellationToken)
                .Unwrap();
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