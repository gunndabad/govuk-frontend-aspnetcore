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
    [RestrictChildren(SummaryCardTitleTagHelper.TagName, SummaryCardActionsTagHelper.TagName, SummaryListTagHelper.TagName)]
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

        internal SummaryCardTagHelper(IGovUkHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var cardContext = new SummaryCardContext();

            using (context.SetScopedContextItem(cardContext))
            {
                await output.GetChildContentAsync();
            }

            cardContext.ThrowIfNotComplete();

            var tagBuilder = _htmlGenerator.GenerateSummaryCard(
                new SummaryCardTitle()
                {
                    Content = cardContext.Title?.Content,
                    HeadingLevel = cardContext.Title?.HeadingLevel,
                    Attributes = cardContext.Title?.Attributes,
                },
                new SummaryListActions()
                {
                    Attributes = cardContext.ActionsAttributes,
                    Items = cardContext.Actions
                },
                cardContext.SummaryList,
                output.Attributes.ToAttributeDictionary());

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
