using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the title in the GDS summary card component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = SummaryCardTagHelper.TagName)]
public class SummaryCardTitleTagHelper : TagHelper
{
    internal const string TagName = "govuk-summary-card-title";

    private int? _headingLevel;

    /// <summary>
    /// The heading level.
    /// </summary>
    /// <remarks>
    /// Must be between <c>1</c> and <c>6</c> (inclusive). The default is <c>2</c>.
    /// </remarks>
    [HtmlAttributeName("heading-level")]
    public int? HeadingLevel
    {
        get => _headingLevel;
        set
        {
            if (value < ComponentGenerator.SummaryCardMinHeadingLevel ||
                value > ComponentGenerator.SummaryCardMaxHeadingLevel)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    $"{nameof(HeadingLevel)} must be between {ComponentGenerator.SummaryCardMinHeadingLevel} and {ComponentGenerator.SummaryCardMaxHeadingLevel}.");
            }

            _headingLevel = value;
        }
    }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var cardContext = context.GetContextItem<SummaryCardContext>();

        var childContent = await output.GetChildContentAsync();

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        cardContext.SetTitle(childContent.Snapshot(), HeadingLevel, output.Attributes.ToAttributeDictionary());

        output.SuppressOutput();
    }
}
