using System;
using System.Collections.Generic;
using System.Text;

namespace EntitiyFrameworkCore.Parallel
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> DisposeAfterEnumeration<T>(this IEnumerable<T> source, IDisposable dbSet)
            => new AutoDisposingEnumerable<T>(source, dbSet);
    }
}
