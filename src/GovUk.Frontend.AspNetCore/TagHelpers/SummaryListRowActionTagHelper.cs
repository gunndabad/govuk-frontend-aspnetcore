using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents an action in a GDS summary list row.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = SummaryListRowActionsTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.SummaryListRowActionElement)]
    public class SummaryListRowActionTagHelper : TagHelper
    {
        internal const string TagName = "govuk-summary-list-row-action";

        private const string VisuallyHiddenTextAttributeName = "visually-hidden-text";

        /// <summary>
        /// The visually hidden text for the action link.
        /// </summary>
        [HtmlAttributeName(VisuallyHiddenTextAttributeName)]
        public string? VisuallyHiddenText { get; set; }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var summaryListRowContext = context.GetContextItem<SummaryListRowContext>();

            var content = await output.GetChildContentAsync();

            summaryListRowContext.AddAction(new SummaryListRowAction()
            {
                Attributes = output.Attributes.ToAttributeDictionary(),
                Content = content.Snapshot(),
                VisuallyHiddenText = VisuallyHiddenText
            });

            output.SuppressOutput();
        }
    }
}
