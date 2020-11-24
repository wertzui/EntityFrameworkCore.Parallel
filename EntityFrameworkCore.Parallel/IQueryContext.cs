using System.Linq.Expressions;
using System.Threading;

namespace EntityFrameworkCore.Parallel
{
    /// <summary>
    /// Interface for a query context. A query context is the thing that will actually execute any query.
    /// </summary>
    public interface IQueryContext
    {
        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        TResult Execute<TResult>(Expression query);

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        TResult ExecuteAsync<TResult>(Expression query, CancellationToken cancellationToken);
    }
}