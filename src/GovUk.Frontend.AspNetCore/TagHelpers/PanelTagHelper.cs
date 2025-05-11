using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS panel component.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(ComponentGenerator.PanelElement)]
[RestrictChildren(PanelTitleTagHelper.TagName, PanelBodyTagHelper.TagName)]
public class PanelTagHelper : TagHelper
{
    internal const string TagName = "govuk-panel";

    private const string HeadingLevelAttributeName = "heading-level";

    private readonly IGovUkHtmlGenerator _htmlGenerator;
    private int _headingLevel = ComponentGenerator.PanelDefaultHeadingLevel;

    /// <summary>
    /// Creates a new <see cref="PanelTagHelper"/>.
    /// </summary>
    public PanelTagHelper()
        : this(null)
    {
    }

    internal PanelTagHelper(IGovUkHtmlGenerator? htmlGenerator = null)
    {
        _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
    }

    /// <summary>
    /// The heading level.
    /// </summary>
    /// <remarks>
    /// Must be between <c>1</c> and <c>6</c> (inclusive). The default is <c>1</c>.
    /// </remarks>
    [HtmlAttributeName(HeadingLevelAttributeName)]
    public int HeadingLevel
    {
        get => _headingLevel;
        set
        {
            if (value < ComponentGenerator.PanelMinHeadingLevel ||
                value > ComponentGenerator.PanelMaxHeadingLevel)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    $"{nameof(HeadingLevel)} must be between {ComponentGenerator.PanelMinHeadingLevel} and {ComponentGenerator.PanelMaxHeadingLevel}.");
            }

            _headingLevel = value;
        }
    }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var panelContext = new PanelContext();

        using (context.SetScopedContextItem(panelContext))
        {
            await output.GetChildContentAsync();
        }

        panelContext.ThrowIfNotComplete();

        var tagBuilder = _htmlGenerator.GeneratePanel(
            HeadingLevel,
            panelContext.Title,
            panelContext.Body,
            output.Attributes.ToAttributeDictionary());

        output.TagName = tagBuilder.TagName;
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Attributes.Clear();
        output.MergeAttributes(tagBuilder);
        output.Content.SetHtmlContent(tagBuilder.InnerHtml);
    }
}
