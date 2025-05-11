using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS skip link component.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(ComponentGenerator.SkipLinkElement)]
public class SkipLinkTagHelper : TagHelper
{
    internal const string TagName = "govuk-skip-link";

    private const string HrefAttributeName = "href";

    private readonly IGovUkHtmlGenerator _htmlGenerator;

    /// <summary>
    /// Creates a new <see cref="BackLinkTagHelper"/>.
    /// </summary>
    public SkipLinkTagHelper()
        : this(htmlGenerator: null)
    {
    }

    internal SkipLinkTagHelper(IGovUkHtmlGenerator? htmlGenerator)
    {
        _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
    }

    /// <summary>
    /// The <c>href</c> attribute for the link.
    /// </summary>
    /// <remarks>
    /// The default is <c>&quot;#content&quot;</c>.
    /// </remarks>
    [HtmlAttributeName(HrefAttributeName)]
    public string? Href { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = await output.GetChildContentAsync();

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        var tagBuilder = _htmlGenerator.GenerateSkipLink(
            Href ?? ComponentGenerator.SkipLinkDefaultHref,
            childContent,
            output.Attributes.ToAttributeDictionary());

        output.TagName = tagBuilder.TagName;
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Attributes.Clear();
        output.MergeAttributes(tagBuilder);
        output.Content.SetHtmlContent(tagBuilder.InnerHtml);
    }
}
