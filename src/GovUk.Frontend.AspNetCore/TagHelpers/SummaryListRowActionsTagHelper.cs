using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the actions wrapper in a GDS summary list component row.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = SummaryListRowTagHelper.TagName)]
    [HtmlTargetElement(TagName, ParentTag = SummaryListRowTagHelper.ShortTagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = SummaryListRowTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = SummaryListRowTagHelper.ShortTagName)]
    [RestrictChildren(SummaryListRowActionTagHelper.TagName, SummaryListRowActionTagHelper.ShortTagName)]
    [OutputElementHint(ComponentGenerator.SummaryListRowActionsElement)]
    public class SummaryListRowActionsTagHelper : TagHelper
    {
        internal const string TagName = "govuk-summary-list-row-actions";
        internal const string ShortTagName = "actions";

        /// <summary>
        /// Creates a new <see cref="SummaryListRowActionsTagHelper"/>.
        /// </summary>
        public SummaryListRowActionsTagHelper()
        {
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var rowContext = context.GetContextItem<SummaryListRowContext>();
            rowContext.SetActionsAttributes(output.Attributes.ToAttributeDictionary());

            await output.GetChildContentAsync();

            output.SuppressOutput();
        }
    }
}
