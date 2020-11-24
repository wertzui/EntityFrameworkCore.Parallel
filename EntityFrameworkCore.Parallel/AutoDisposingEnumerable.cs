using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Parallel
{
    /// <summary>
    /// An <see cref="IEnumerable{T}"/> which will dispose the given disposable object once fully enumerated.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    public class AutoDisposingEnumerable<T> : IEnumerable<T>
    {
        private readonly IDisposable _disposable;
        private readonly IEnumerable<T> _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoDisposingEnumerable{T}"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="disposable">The disposable.</param>
        /// <exception cref="ArgumentNullException">
        /// source
        /// or
        /// disposable
        /// </exception>
        public AutoDisposingEnumerable(IEnumerable<T> source, IDisposable disposable)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _disposable = disposable ?? throw new ArgumentNullException(nameof(disposable));
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return new AutoDisposingEnumerator<T>(_source, _disposable);
        }

        /// <inheritdoc/>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
