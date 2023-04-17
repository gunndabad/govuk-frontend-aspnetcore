using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the value in a GDS summary list component row.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = SummaryListRowTagHelper.TagName)]
    [HtmlTargetElement(TagName, ParentTag = SummaryListRowTagHelper.ShortTagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = SummaryListRowTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = SummaryListRowTagHelper.ShortTagName)]
    [OutputElementHint(ComponentGenerator.SummaryListRowValueElement)]
    public class SummaryListRowValueTagHelper : TagHelper
    {
        internal const string TagName = "govuk-summary-list-row-value";
        internal const string ShortTagName = "value";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var summaryListRowContext = context.GetContextItem<SummaryListRowContext>();

            var content = await output.GetChildContentAsync();

            summaryListRowContext.SetValue(output.Attributes.ToAttributeDictionary(), content.Snapshot());

            output.SuppressOutput();
        }
    }
}
