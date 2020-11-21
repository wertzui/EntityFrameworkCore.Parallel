using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EntitiyFrameworkCore.Parallel
{
    /// <summary>
    /// An <see cref="IEnumerable{T}"/> which will dispose the given disposable object once fully enumerated.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    public class AutoDisposingEnumerable<T> : IEnumerable<T>
    {
        private readonly IDisposable _disposable;
        private readonly IEnumerable<T> _source;

        public AutoDisposingEnumerable(IEnumerable<T> source, IDisposable disposable)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _disposable = disposable ?? throw new ArgumentNullException(nameof(disposable));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new AutoDisposingEnumerator<T>(_source, _disposable);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
