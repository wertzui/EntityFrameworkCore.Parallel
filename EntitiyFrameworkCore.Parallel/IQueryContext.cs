using System.Linq.Expressions;
using System.Threading;

namespace EntitiyFrameworkCore.Parallel
{
    /// <summary>
    /// Interface for a query context. A query context is the thing that will actually execute any query.
    /// </summary>
    public interface IQueryContext
    {
        TResult Execute<TResult>(Expression query);
        TResult ExecuteAsync<TResult>(Expression query, CancellationToken cancellationToken);
    }
}
