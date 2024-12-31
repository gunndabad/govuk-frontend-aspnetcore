using System.Collections.Generic;
using System.Collections.Immutable;
using System.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore;

/// <summary>
/// Utility extensions for <see cref="TagHelperAttributeList"/>.
/// </summary>
public static class TagHelperAttributeListExtensions
{
    /// <summary>
    /// Creates an <see cref="AttributeDictionary"/> from a <see cref="TagHelperAttributeList"/>.
    /// </summary>
    /// <param name="list">The <see cref="TagHelperAttributeList"/> to retrieve attributes from.</param>
    public static AttributeDictionary ToAttributeDictionary(this TagHelperAttributeList? list)
    {
        var attributeDictionary = new AttributeDictionary();

        if (list != null)
        {
            foreach (var attribute in list)
            {
                attributeDictionary.Add(
                    attribute.Name,
                    attribute.ValueStyle == HtmlAttributeValueStyle.Minimized ?
                        string.Empty :
                        attribute.Value is HtmlString htmlString ? HttpUtility.HtmlDecode(htmlString.Value) :
                        (attribute.Value ?? string.Empty).ToString());
            }
        }

        return attributeDictionary;
    }

    internal static ImmutableDictionary<string, string?> ToEncodedAttributeDictionary(this TagHelperAttributeList? list)
    {
        var attributeDictionary = new Dictionary<string, string?>();

        if (list != null)
        {
            foreach (var attribute in list)
            {
                attributeDictionary.Add(
                    attribute.Name,
                    attribute.ValueStyle == HtmlAttributeValueStyle.Minimized ?
                        null :
                        attribute.Value is HtmlString htmlString ? htmlString.ToString() :
                        (attribute.Value ?? string.Empty).ToString());
            }
        }

        return attributeDictionary.ToImmutableDictionary();
    }
}
