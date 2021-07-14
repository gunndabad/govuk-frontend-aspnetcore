using System;
using System.Collections.Generic;
using System.Linq;

namespace GovUk.Frontend.AspNetCore
{
    public static class EnumerableExtensions
    {
#if NETSTANDARD2_0
        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, int count)
        {
            Guard.ArgumentNotNull(nameof(source), source);

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var array = source.ToArray();

            return array.Take(Math.Max(0, array.Length - count));
        }

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count)
        {
            Guard.ArgumentNotNull(nameof(source), source);

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var array = source.ToArray();

            return array.Skip(Math.Max(0, array.Length - count));
        }
#endif
    }
}
