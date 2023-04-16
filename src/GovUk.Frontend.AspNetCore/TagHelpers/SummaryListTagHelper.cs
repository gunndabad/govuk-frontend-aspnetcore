using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Generates a GDS summary list component.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [RestrictChildren(SummaryListRowTagHelper.TagName, SummaryListRowTagHelper.ShortTagName)]
    [OutputElementHint(ComponentGenerator.SummaryListElement)]
    public class SummaryListTagHelper : TagHelper
    {
        internal const string TagName = "govuk-summary-list";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a new <see cref="SummaryListTagHelper"/>.
        /// </summary>
        public SummaryListTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal SummaryListTagHelper(IGovUkHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var summaryListContext = new SummaryListContext();

            using (context.SetScopedContextItem(summaryListContext))
            {
                await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateSummaryList(
                output.Attributes.ToAttributeDictionary(),
                summaryListContext.Rows);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
