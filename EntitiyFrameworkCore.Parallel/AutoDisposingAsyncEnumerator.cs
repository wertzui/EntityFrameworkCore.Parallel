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

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoDisposingAsyncEnumerator{T}"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="asyncDisposable">The asynchronous disposable.</param>
        /// <exception cref="ArgumentNullException">
        /// source
        /// or
        /// asyncDisposable
        /// </exception>
        public AutoDisposingAsyncEnumerator(IAsyncEnumerable<T> source, IAsyncDisposable asyncDisposable)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            _enumerator = source.GetAsyncEnumerator();
            _asyncDisposable = asyncDisposable ?? throw new ArgumentNullException(nameof(asyncDisposable));
        }
        /// <inheritdoc/>
        public T Current => _enumerator.Current;

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            await _enumerator.DisposeAsync();
            await _asyncDisposable.DisposeAsync();
        }

        /// <inheritdoc/>
        public ValueTask<bool> MoveNextAsync() => _enumerator.MoveNextAsync();
    }
}
