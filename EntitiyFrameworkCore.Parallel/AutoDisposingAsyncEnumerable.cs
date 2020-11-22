using System;
using System.Collections.Generic;
using System.Threading;

namespace EntitiyFrameworkCore.Parallel
{
    /// <summary>
    /// An <see cref="IAsyncEnumerable{T}"/> which will dispose the given disposable object once fully enumerated.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    public class AutoDisposingAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly IAsyncDisposable _asyncDisposable;
        private readonly IAsyncEnumerable<T> _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoDisposingAsyncEnumerable{T}"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="dbSet">The database set.</param>
        /// <exception cref="ArgumentNullException">
        /// source
        /// or
        /// dbSet
        /// </exception>
        public AutoDisposingAsyncEnumerable(IAsyncEnumerable<T> source, IAsyncDisposable dbSet)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _asyncDisposable = dbSet ?? throw new ArgumentNullException(nameof(dbSet));
        }

        /// <inheritdoc/>
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) => new AutoDisposingAsyncEnumerator<T>(_source, _asyncDisposable);
    }
}