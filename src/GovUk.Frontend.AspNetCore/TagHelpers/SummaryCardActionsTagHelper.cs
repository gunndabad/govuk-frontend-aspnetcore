using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the actions wrapper in a GDS summary card.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = SummaryCardTagHelper.TagName)]
[RestrictChildren(SummaryCardActionTagHelper.TagName)]
public class SummaryCardActionsTagHelper : TagHelper
{
    internal const string TagName = "govuk-summary-card-actions";

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var cardContext = context.GetContextItem<SummaryCardContext>();
        cardContext.SetActionsAttributes(output.Attributes.ToAttributeDictionary());

        await output.GetChildContentAsync();

        output.SuppressOutput();
    }
}
