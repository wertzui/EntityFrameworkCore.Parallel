using System;
using System.Collections.Generic;

namespace EntityFrameworkCore.Parallel
{
    /// <summary>
    /// Contains extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Disposes the <paramref name="dbSet"/> after enumeration.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="dbSet">The database set.</param>
        /// <returns></returns>
        public static IEnumerable<T> DisposeAfterEnumeration<T>(this IEnumerable<T> source, IDisposable dbSet)
            => new AutoDisposingEnumerable<T>(source, dbSet);
    }
}
