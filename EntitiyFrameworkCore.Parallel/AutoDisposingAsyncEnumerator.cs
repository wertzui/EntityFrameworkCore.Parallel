using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntitiyFrameworkCore.Parallel
{
    /// <summary>
    /// This async enumerator will automatically dispose the given async disposable object once fully enumerated.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    public class AutoDisposingAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IAsyncDisposable _asyncDisposable;
        private readonly IAsyncEnumerator<T> _enumerator;

        public AutoDisposingAsyncEnumerator(IAsyncEnumerable<T> source, IAsyncDisposable asyncDisposable)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            _enumerator = source.GetAsyncEnumerator();
            _asyncDisposable = asyncDisposable ?? throw new ArgumentNullException(nameof(asyncDisposable));
        }
        public T Current => _enumerator.Current;

        public async ValueTask DisposeAsync()
        {
            await _enumerator.DisposeAsync();
            await _asyncDisposable.DisposeAsync();
        }

        public ValueTask<bool> MoveNextAsync() => _enumerator.MoveNextAsync();
    }
}
