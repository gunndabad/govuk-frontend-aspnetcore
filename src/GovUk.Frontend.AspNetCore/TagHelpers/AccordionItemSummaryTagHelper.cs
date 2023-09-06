using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the summary in a GDS accordion component item.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = AccordionItemTagHelper.TagName)]
[OutputElementHint(ComponentGenerator.AccordionItemSummaryElement)]
public class AccordionItemSummaryTagHelper : TagHelper
{
    internal const string TagName = "govuk-accordion-item-summary";

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var itemContext = context.GetContextItem<AccordionItemContext>();

        var childContent = await output.GetChildContentAsync();

        itemContext.SetSummary(output.Attributes.ToAttributeDictionary(), childContent.Snapshot());

        output.SuppressOutput();
    }
}
