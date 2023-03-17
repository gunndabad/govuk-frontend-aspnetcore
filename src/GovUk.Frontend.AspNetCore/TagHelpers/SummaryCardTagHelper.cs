using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Generates a GOV.UK summary card component.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [RestrictChildren(SummaryListTagHelper.TagName, SummaryCardTitleTagHelper.TagName, SummaryCardActionsTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.SummaryCardElement)]
    public class SummaryCardTagHelper : TagHelper
    {
        internal const string TagName = "govuk-summary-card";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a new <see cref="SummaryCardTagHelper"/>.
        /// </summary>
        public SummaryCardTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal SummaryCardTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var cardContext = new SummaryCardContext();

            TagHelperContent summaryList = null;
            using (context.SetScopedContextItem(cardContext))
            {
                summaryList = await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateSummaryCard(new SummaryCard
            {
                CardAttributes = output.Attributes.ToAttributeDictionary(),
                TitleContent = cardContext.Title?.Content,
                TitleAttributes = cardContext.Title?.Attributes,
                HeadingLevel = cardContext.HeadingLevel,
                SummaryList = summaryList?.Snapshot(),
                Actions = new SummaryCardActions
                {
                    Items = cardContext.Actions,
                    Attributes = cardContext.ActionsAttributes
                }
            });

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
