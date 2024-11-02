using System;
using System.Collections.Generic;
using System.Linq;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

internal static class CollectionExtensions
{
    public static IDictionary<TKey, TValue> OrEmpty<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) =>
        dictionary ?? new Dictionary<TKey, TValue>();

    public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> list) => list ?? Enumerable.Empty<T>();

    public static IList<T> OrEmpty<T>(this IList<T> list) => list ?? Array.Empty<T>();
}
