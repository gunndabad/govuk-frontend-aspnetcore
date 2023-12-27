using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the summary in a GDS details component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = DetailsTagHelper.TagName)]
[OutputElementHint(DefaultComponentGenerator.DetailsSummaryElement)]
public class DetailsSummaryTagHelper : TagHelper
{
    internal const string TagName = "govuk-details-summary";

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var detailsContext = context.GetContextItem<DetailsContext>();

        var content = await output.GetChildContentAsync();

        if (output.Content.IsModified)
        {
            content = output.Content;
        }

        detailsContext.SetSummary(output.Attributes.ToEncodedAttributeDictionary(), content.ToHtmlString());

        output.SuppressOutput();
    }
}
