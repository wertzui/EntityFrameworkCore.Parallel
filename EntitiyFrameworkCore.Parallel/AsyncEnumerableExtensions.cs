using System;
using System.Collections.Generic;

namespace EntitiyFrameworkCore.Parallel
{
    public static class AsyncEnumerableExtensions
    {
        public static IAsyncEnumerable<T> Buffer<T>(this IAsyncEnumerable<T> source, IAsyncDisposable dbSet)
            => new BufferedAsyncEnumerable<T>(source, dbSet);
    }
}
