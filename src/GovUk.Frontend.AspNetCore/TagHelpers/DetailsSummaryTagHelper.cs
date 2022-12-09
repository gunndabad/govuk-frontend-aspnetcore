using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the summary in a GDS details component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = DetailsTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.DetailsSummaryElement)]
    public class DetailsSummaryTagHelper : TagHelper
    {
        internal const string TagName = "govuk-details-summary";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            var detailsContext = context.GetContextItem<DetailsContext>();
            detailsContext.SetSummary(output.Attributes.ToAttributeDictionary(), childContent.Snapshot());

            output.SuppressOutput();
        }
    }
}
