using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public static class TagBuilderExtensions
{
    public static void AddAttribute(this TagBuilder tagBuilder, string key, bool value)
    {
        tagBuilder.Attributes.Add(key, value ? "true" : "false");
    }

    public static void AddAttributes(this TagBuilder tagBuilder, IDictionary<string, object> attributes)
    {
        if (attributes == null)
        {
            return;
        }

        foreach (var kvp in attributes)
        {
            var value = kvp.Value.ToString();

            if (kvp.Value is bool boolValue)
            {
                value = boolValue ? "true" : "false";
            }

            tagBuilder.Attributes.Add(kvp.Key, value);
        }
    }
}
