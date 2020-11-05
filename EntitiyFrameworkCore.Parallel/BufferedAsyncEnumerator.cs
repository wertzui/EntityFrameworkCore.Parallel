using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntitiyFrameworkCore.Parallel
{
    /// <summary>
    /// This enumerator basically wraps the buffer (the list) back into an <see cref="IAsyncEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    public class BufferedAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly ValueTask<List<T>> _listTask;
        private readonly IAsyncDisposable _dbSet;
        private IEnumerator<T> _listEnumerator;

        public BufferedAsyncEnumerator(ValueTask<List<T>> listTask, IAsyncDisposable dbSet)
        {
            _listTask = listTask;
            _dbSet = dbSet;
        }
        public T Current => _listEnumerator is null ? default : _listEnumerator.Current;

        public async ValueTask DisposeAsync()
        {
            if (_listEnumerator is IAsyncDisposable asyncEnumerator)
                await asyncEnumerator.DisposeAsync();
            else
                _listEnumerator.Dispose();

            await _dbSet.DisposeAsync();
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            if (_listEnumerator is null)
            {
                var list = await _listTask;
                _listEnumerator = list.GetEnumerator();
            }

            return _listEnumerator.MoveNext();
        }
    }
}
