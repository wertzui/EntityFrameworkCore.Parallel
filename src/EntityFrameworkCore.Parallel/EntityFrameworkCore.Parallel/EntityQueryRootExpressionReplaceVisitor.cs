using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EntityFrameworkCore.Parallel
{
    /// <summary>
    /// A <see cref="ExpressionVisitor"/> that will replace each <see cref="EntityQueryRootExpression"/> with the one given to it.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="ExpressionVisitor" />
    public class EntityQueryRootExpressionReplaceVisitor<TEntity> : ExpressionVisitor
         where TEntity : class
    {
        /// <summary>
        /// Gets a value indicating whether the query was replaced.
        /// </summary>
        public bool QueryWasReplaced { get; private set; }

        private readonly Expression _query;
        private readonly DbSet<TEntity> _set;
        private readonly IQueryProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityQueryRootExpressionReplaceVisitor{TEntity}"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="set">The set.</param>
        /// <param name="provider">The provider of the set.</param>
        public EntityQueryRootExpressionReplaceVisitor(Expression query, DbSet<TEntity> set, IQueryProvider provider)
        {
            _query = query;
            _set = set;
            _provider = provider;
        }

        /// <inheritdoc/>
        public override Expression? Visit(Expression? node)
        {
            if (node is EntityQueryRootExpression root)
            {
                QueryWasReplaced = true;
                var replacedQuery = CreateQueryWithNewProvider(root, _set, _provider);
                return replacedQuery;
            }

            return base.Visit(node);
        }

        /// <summary>
        /// Creates a new query of the same type as the given query but with the new provider.
        /// </summary>
        private static EntityQueryRootExpression CreateQueryWithNewProvider(EntityQueryRootExpression query, DbSet<TEntity> set, IQueryProvider provider)
        {
            GetConstructorWithMostParameters(query, provider, out var queryType, out var constructorWithMostParameters, out var constructorParameters);

            var parameters = CreateConstructorParameterValues(query, set, provider, queryType, constructorParameters);

            var replaced = (EntityQueryRootExpression)constructorWithMostParameters.Invoke(parameters);

            return replaced;
        }

        private static object?[] CreateConstructorParameterValues(EntityQueryRootExpression query, DbSet<TEntity> set, IQueryProvider provider, Type queryType, ParameterInfo[] constructorParameters)
        {
            return constructorParameters
                .Select(p => p.ParameterType switch
                    {
                        var t when t == typeof(IAsyncQueryProvider) => (IAsyncQueryProvider)provider,
                        var t when t == typeof(IEntityType) => set.EntityType,
                        _ => queryType.GetProperty(p.Name!, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase)?.GetValue(query)
                    }
                )
                .ToArray();
        }

        private static void GetConstructorWithMostParameters(
            EntityQueryRootExpression query,
            IQueryProvider provider,
            out Type queryType,
            out ConstructorInfo constructorWithMostParameters,
            out ParameterInfo[] constructorParameters)
        {
            queryType = query.GetType();
            constructorWithMostParameters = queryType.GetConstructors()
                .Where(c => provider is not IAsyncQueryProvider || c.GetParameters().Any(p => p.ParameterType == typeof(IAsyncQueryProvider)))
                .OrderByDescending(c => c.GetParameters().Length)
                .First();
            constructorParameters = constructorWithMostParameters.GetParameters();
        }
    }

    /// <summary>
    /// Provides a way to replace the provider of an expression with a given one.
    /// </summary>
    public static class EntityQueryRootExpressionReplaceVisitor
    {
        /// <summary>
        /// Replaces the provider of the expression with the given one.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="set">The set.</param>
        /// <param name="provider">The provider of the set.</param>
        /// <returns>The replaced expression.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="query"/>, <paramref name="set"/>, or <paramref name="provider"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the supplied query does not originate from an <see cref="EntityQueryRootExpression"/> and therefore does not come from Entity Framework.
        /// </exception>
        public static Expression ReplaceProvider<TEntity>(Expression query, DbSet<TEntity> set, IQueryProvider provider)
            where TEntity : class
        {
            ArgumentNullException.ThrowIfNull(query);
            ArgumentNullException.ThrowIfNull(set);
            ArgumentNullException.ThrowIfNull(provider);

            var visitor = new EntityQueryRootExpressionReplaceVisitor<TEntity>(query, set, provider);
            var replaced = visitor.Visit(query);

            if (!visitor.QueryWasReplaced)
                throw new ArgumentException($"The supplied query does not originate from an {nameof(EntityQueryRootExpression)} and therefore does not come from Entity Framework.", nameof(query));

            return replaced ?? throw new ArgumentException($"The supplied query resulted in a replaced expression which was null. This should not happen.", nameof(query));
        }
    }
}
