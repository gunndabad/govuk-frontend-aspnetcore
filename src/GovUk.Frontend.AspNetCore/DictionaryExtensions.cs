using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore;

internal static class DictionaryExtensions
{
    public static AttributeDictionary ToAttributeDictionary(this IDictionary<string, string?>? dictionary)
    {
        var attributeDictionary = new AttributeDictionary();

        if (dictionary is not null)
        {
            foreach (var kvp in dictionary)
            {
                attributeDictionary.Add(kvp.Key, kvp.Value);
            }
        }

        return attributeDictionary;
    }

    public static ImmutableDictionary<TKey, TValue> ToImmutableDictionary<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue>? dict)
        where TKey : notnull
    {
        return dict is not null ? ImmutableDictionary.ToImmutableDictionary(dict) : ImmutableDictionary<TKey, TValue>.Empty;
    }

    public static ImmutableDictionary<TKey, TValue> AddIf<TKey, TValue>(this ImmutableDictionary<TKey, TValue> dict, bool condition, TKey key, TValue value)
        where TKey : notnull
    {
        if (condition)
        {
            return dict.Add(key, value);
        }

        return dict;
    }

    public static ImmutableDictionary<TKey, TValue> AddIfNotNull<TKey, TValue>(this ImmutableDictionary<TKey, TValue> dict, TKey key, TValue value)
        where TKey : notnull
    {
        return AddIf<TKey, TValue>(dict, value is not null, key, value);
    }

    public static ImmutableDictionary<TKey, TValue> Remove<TKey, TValue>(this ImmutableDictionary<TKey, TValue> dict, TKey key, out TValue? value)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dict);

        dict.TryGetValue(key, out value);
        return dict.Remove(key);
    }
}
