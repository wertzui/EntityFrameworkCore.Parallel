using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EntitiyFrameworkCore.Parallel
{
    /// <summary>
    /// A <see cref="ExpressionVisitor"/> that will replace each <see cref="QueryRootExpression"/> with the one given to it.
    /// </summary>
    /// <seealso cref="System.Linq.Expressions.ExpressionVisitor" />
    public class QueryRootExpressionReplaceVisitor : ExpressionVisitor
    {
        private readonly QueryRootExpression _setQuery;
        private bool queryWasReplaced;

        /// <summary>
        /// Replaces the provider of the expression with the given one.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="set">The set.</param>
        /// <param name="provider">The provider of the set.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// query
        /// or
        /// set
        /// or
        /// provider
        /// </exception>
        /// <exception cref="ArgumentException">The supplied query does not originate from a QueryRootExpression and therefor does not come from Entity Framework. - query</exception>
        public static Expression ReplaceProvider<TEntity>(Expression query, DbSet<TEntity> set, IQueryProvider provider)
            where TEntity : class
        {
            if (query is null)
                throw new ArgumentNullException(nameof(query));
            if (set is null)
                throw new ArgumentNullException(nameof(set));
            if (provider is null)
                throw new ArgumentNullException(nameof(provider));

            var setQuery = provider is IAsyncQueryProvider asyncProvider ?
                new QueryRootExpression(asyncProvider, set.EntityType) :
                new QueryRootExpression(set.EntityType);

            var visitor = new QueryRootExpressionReplaceVisitor(setQuery);
            var replaced = visitor.Visit(query);

            if (!visitor.queryWasReplaced)
                throw new ArgumentException($"The supplied query does not originate from a {nameof(QueryRootExpression)} and therefor does not come from Entity Framework.", nameof(query));

            return replaced;
        }

        private QueryRootExpressionReplaceVisitor(QueryRootExpression setQuery)
        {
            _setQuery = setQuery;
        }

        /// <inheritdoc/>
        public override Expression Visit(Expression node)
        {
            if (node is QueryRootExpression)
            {
                queryWasReplaced = true;
                return _setQuery;
            }

            return base.Visit(node);
        }
    }
}