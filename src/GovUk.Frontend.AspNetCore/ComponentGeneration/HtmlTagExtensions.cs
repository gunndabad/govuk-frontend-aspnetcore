using System;
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

    public static HtmlTag AddBooleanAttributeIf(this HtmlTag tag, bool condition, string key)
    {
        return condition ? tag.BooleanAttr(key) : tag;
    }

    public static HtmlTag AddEncodedAttributeIf(this HtmlTag tag, bool condition, string key, string? value)
    {
        return condition ? tag.UnencodedAttr(key, value) : tag;
    }

    public static HtmlTag AddEncodedAttributeIfNotNull(this HtmlTag tag, string key, string? value)
    {
        return AddEncodedAttributeIf(tag, value is not null, key, value);
    }

    public static HtmlTag AppendIf(this HtmlTag tag, bool condition, Func<HtmlTag> createChild)
    {
        return condition ? tag.Append(createChild()) : tag;
    }

    public static HtmlTag AppendHtmlIf(this HtmlTag tag, bool condition, Func<string> createHtml)
    {
        return condition ? tag.AppendHtml(createHtml()) : tag;
    }

    public static HtmlTag AppendHtmlIf(this HtmlTag tag, bool condition, string html)
    {
        return AppendHtmlIf(tag, condition, () => html);
    }

    public static HtmlTag AddClassIf(this HtmlTag tag, bool condition, string className)
    {
        return condition ? tag.AddClass(className) : tag;
    }
}
