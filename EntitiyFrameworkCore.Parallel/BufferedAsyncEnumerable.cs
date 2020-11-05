using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EntitiyFrameworkCore.Parallel
{
    /// <summary>
    /// An <see cref="IAsyncEnumerable{T}"/> which buffers the result in a list.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    public class BufferedAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly IAsyncDisposable _dbSet;
        private ValueTask<List<T>> _list;

        public BufferedAsyncEnumerable(IAsyncEnumerable<T> source, IAsyncDisposable dbSet)
        {
            _list = source.ToListAsync();
            _dbSet = dbSet;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new BufferedAsyncEnumerator<T>(_list, _dbSet);
        }
    }
}
