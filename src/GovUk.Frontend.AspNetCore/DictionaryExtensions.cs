using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore;

internal static class DictionaryExtensions
{
    public static AttributeDictionary ToAttributeDictionary(this IDictionary<string, string?>? dictionary)
    {
        var attributeDictionary = new AttributeDictionary();

        if (dictionary != null)
        {
            foreach (var kvp in dictionary)
            {
                attributeDictionary.Add(kvp.Key, kvp.Value);
            }
        }

        return attributeDictionary;
    }
}
