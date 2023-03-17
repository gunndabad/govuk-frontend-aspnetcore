using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents an action in a GDS summary card.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = SummaryCardActionsTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.SummaryCardActionElement)]
    public class SummaryCardActionTagHelper : TagHelper
    {
        internal const string TagName = "govuk-summary-card-action";

        private const string VisuallyHiddenTextAttributeName = "visually-hidden-text";

        /// <summary>
        /// The destination URL for the action link.
        /// </summary>
        [HtmlAttributeName("href")]
        public string Href { get; set; }

        /// <summary>
        /// The visually hidden text for the action link.
        /// </summary>
        [HtmlAttributeName(VisuallyHiddenTextAttributeName)]
        public string VisuallyHiddenText { get; set; }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var cardContext = context.GetContextItem<SummaryCardContext>();

            var content = await output.GetChildContentAsync();

            cardContext.AddAction(new SummaryCardAction
            {
                Attributes = output.Attributes.ToAttributeDictionary(),
                Content = content.Snapshot(),
                VisuallyHiddenText = VisuallyHiddenText,
                Href = Href
            });

            output.SuppressOutput();
        }
    }
}
