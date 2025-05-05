using System;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SoftCircuits.HtmlMonkey;

namespace GovUk.Frontend.AspNetCore.Components;

internal static class TagHelperAdapter
{
    public static void ApplyComponentHtml(this TagHelperOutput output, IHtmlContent? content)
    {
        ArgumentNullException.ThrowIfNull(output);

        var unwrapped = UnwrapComponent(content);

        output.TagName = unwrapped.TagName;
        output.TagMode = unwrapped.TagMode;

        output.Attributes.Clear();

        foreach (var attribute in unwrapped.Attributes)
        {
            output.Attributes.Add(attribute);
        }

        output.Content.AppendHtml(unwrapped.InnerHtml);
    }

    internal static ComponentTagHelperOutput UnwrapComponent(string? html) =>
        UnwrapComponent(new HtmlString(html));

    internal static ComponentTagHelperOutput UnwrapComponent(IHtmlContent? content)
    {
        ArgumentNullException.ThrowIfNull(content);

        var html = content.ToHtmlString();

        if (string.IsNullOrWhiteSpace(html))
        {
            return ComponentTagHelperOutput.Empty;
        }

        var doc = HtmlDocument.FromHtml(html);
        var root = (HtmlElementNode)doc.RootNodes.Single();

        var tagName = root.TagName;
        var tagMode = root.IsSelfClosing ? TagMode.SelfClosing : TagMode.StartTagAndEndTag;
        var attributes = new TagHelperAttributeList(
            root.Attributes.Select(a => a.Value is null ? new TagHelperAttribute(a.Name) : new TagHelperAttribute(a.Name, a.Value)));
        var innerHtml = new HtmlString(root.InnerHtml);

        return new ComponentTagHelperOutput(tagName, tagMode, attributes, innerHtml);
    }

    internal record ComponentTagHelperOutput(
        string? TagName,
        TagMode TagMode,
        ReadOnlyTagHelperAttributeList Attributes,
        IHtmlContent InnerHtml)
    {
        public static ReadOnlyTagHelperAttributeList EmptyAttributes { get; } =
            new TagHelperAttributeList();

        public static IHtmlContent EmptyContent { get; } = new HtmlString("");

        public static ComponentTagHelperOutput Empty { get; } =
            new(null, TagMode.StartTagAndEndTag, EmptyAttributes, EmptyContent);
    }
}
