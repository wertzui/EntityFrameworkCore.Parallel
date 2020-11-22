using System;
using System.Collections.Generic;

namespace EntitiyFrameworkCore.Parallel
{
    /// <summary>
    /// Contains extension methods for <see cref="IAsyncEnumerable{T}"/>.
    /// </summary>
    public static class AsyncEnumerableExtensions
    {
        /// <summary>
        /// Disposes the <paramref name="dbSet"/> after enumeration.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="dbSet">The database set.</param>
        /// <returns></returns>
        public static IAsyncEnumerable<T> DisposeAfterEnumeration<T>(this IAsyncEnumerable<T> source, IAsyncDisposable dbSet)
            => new AutoDisposingAsyncEnumerable<T>(source, dbSet);
    }
}
