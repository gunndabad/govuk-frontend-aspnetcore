using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS warning text component.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(ComponentGenerator.WarningTextElement)]
public class WarningTextTagHelper : TagHelper
{
    internal const string TagName = "govuk-warning-text";

    private const string IconFallbackTextAttributeName = "icon-fallback-text";

    private readonly IGovUkHtmlGenerator _htmlGenerator;

    /// <summary>
    /// Creates a new <see cref="WarningTextTagHelper"/>.
    /// </summary>
    public WarningTextTagHelper()
        : this(htmlGenerator: null)
    {
    }

    internal WarningTextTagHelper(IGovUkHtmlGenerator? htmlGenerator = null)
    {
        _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
    }

    /// <summary>
    /// The fallback text for the icon.
    /// </summary>
    [HtmlAttributeName(IconFallbackTextAttributeName)]
    public string? IconFallbackText { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = await output.GetChildContentAsync();

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        var tagBuilder = _htmlGenerator.GenerateWarningText(
            IconFallbackText ?? ComponentGenerator.WarningTextDefaultIconFallbackText,
            childContent,
            output.Attributes.ToAttributeDictionary());

        output.TagName = tagBuilder.TagName;
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Attributes.Clear();
        output.MergeAttributes(tagBuilder);
        output.Content.SetHtmlContent(tagBuilder.InnerHtml);
    }
}
