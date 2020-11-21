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
        private IEnumerator<T> _enumerator;

        public AutoDisposingEnumerator(IEnumerable<T> source, IDisposable disposable)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            _enumerator = source.GetEnumerator();
            _disposable = disposable ?? throw new ArgumentNullException(nameof(disposable));
        }

        public T Current => _enumerator.Current;

        object System.Collections.IEnumerator.Current => Current;

        public void Dispose()
        {
            _enumerator.Dispose();
            _disposable.Dispose();
        }

        public bool MoveNext() => _enumerator.MoveNext();

        public void Reset() => _enumerator.Reset();
    }
}