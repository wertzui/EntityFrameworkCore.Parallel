using System;
using System.Collections.Generic;

namespace EntitiyFrameworkCore.Parallel
{
    /// <summary>
    /// This enumerator will automatically dispose the given disposable object once fully enumerated.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    public class AutoDisposingEnumerator<T> : IEnumerator<T>
    {
        private readonly IDisposable _disposable;
        private readonly IEnumerator<T> _enumerator;

        /// <summary>Initializes a new instance of the <see cref="AutoDisposingEnumerator{T}" /> class.</summary>
        /// <param name="source">The source.</param>
        /// <param name="disposable">The disposable.</param>
        /// <exception cref="ArgumentNullException">source
        /// or
        /// disposable</exception>
        public AutoDisposingEnumerator(IEnumerable<T> source, IDisposable disposable)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            _enumerator = source.GetEnumerator();
            _disposable = disposable ?? throw new ArgumentNullException(nameof(disposable));
        }

        /// <inheritdoc/>
        public T Current => _enumerator.Current;

        /// <inheritdoc/>
        object System.Collections.IEnumerator.Current => Current;

        /// <inheritdoc/>
        public void Dispose()
        {
            _enumerator.Dispose();
            _disposable.Dispose();

            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public bool MoveNext() => _enumerator.MoveNext();

        /// <inheritdoc/>
        public void Reset() => _enumerator.Reset();
    }
}