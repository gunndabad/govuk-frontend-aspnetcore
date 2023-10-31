using System.Collections.Generic;
using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

internal static class HtmlTagExtensions
{
    public static HtmlTag MergeEncodedAttributes(this HtmlTag tag, IDictionary<string, string?>? attributes)
    {
        if (attributes is null)
        {
            return tag;
        }

        foreach (var attr in attributes)
        {
            if (attr.Value is null)
            {
                tag.BooleanAttr(attr.Key);
            }
            else
            {
                tag.UnencodedAttr(attr.Key, attr.Value);
            }
        }

        return tag;
    }
}
