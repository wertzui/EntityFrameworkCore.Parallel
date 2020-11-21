using System;
using System.Collections.Generic;

namespace EntitiyFrameworkCore.Parallel
{
    public static class AsyncEnumerableExtensions
    {
        public static IAsyncEnumerable<T> DisposeAfterEnumeration<T>(this IAsyncEnumerable<T> source, IAsyncDisposable dbSet)
            => new AutoDisposingAsyncEnumerable<T>(source, dbSet);
    }
}
