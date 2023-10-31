using System;
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

internal static class DictionaryExtensions
{
    public static Dictionary<TKey, TValue> AddIf<TKey, TValue>(this Dictionary<TKey, TValue> dict, bool condition, TKey key, TValue value)
        where TKey : notnull
    {
        if (condition)
        {
            dict.Add(key, value);
        }

        return dict;
    }

    public static Dictionary<TKey, TValue> AddIfNotNull<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        where TKey : notnull
    {
        if (value is not null)
        {
            dict.Add(key, value);
        }

        return dict;
    }

    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue>? first, Dictionary<TKey, TValue>? second)
        where TKey : notnull
    {
        var result = first is not null ? new Dictionary<TKey, TValue>(first) : new Dictionary<TKey, TValue>();

        if (second is not null)
        {
            foreach (var kvp in second)
            {
                result.Add(kvp.Key, kvp.Value);
            }
        }

        return result;
    }
}
