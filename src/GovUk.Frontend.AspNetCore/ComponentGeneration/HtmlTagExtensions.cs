using System.Collections.Generic;
using HtmlTags;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

internal static class HtmlTagExtensions
{
    public static HtmlTag MergeEncodedAttributes(this HtmlTag tag, IReadOnlyDictionary<string, string?>? attributes)
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

    public static void WriteComponent(this TagHelperOutput output, HtmlTag tag)
    {
        output.TagName = null;
        output.Attributes.Clear();
        output.Content.AppendHtml(tag.ToHtmlString());
    }
    
    public static HtmlTag AddEncodedAttributeIf(this HtmlTag tag, bool condition, string key, string? value)
    {
        return condition ? tag.UnencodedAttr(key, value) : tag;
    }

    public static HtmlTag AddEncodedAttributeIfNotNull(this HtmlTag tag, string key, string? value)
    {
        return AddEncodedAttributeIf(tag, value is not null, key, value);
    }

    public static HtmlTag AppendIf(this HtmlTag tag, bool condition, HtmlTag child)
    {
        return condition ? tag.Append(child) : tag;
    }
}
