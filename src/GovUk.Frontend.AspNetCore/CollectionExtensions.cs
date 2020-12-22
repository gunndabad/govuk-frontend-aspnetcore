#nullable enable
#if !NETCOREAPP3_1
using System;
using System.Collections.Generic;
using System.Text;

namespace GovUk.Frontend.AspNetCore
{
    internal static class CollectionExtensions
    {
        public static TValue? GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.GetValueOrDefault(key, default!);
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            TValue? value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }
    }
}
#endif
