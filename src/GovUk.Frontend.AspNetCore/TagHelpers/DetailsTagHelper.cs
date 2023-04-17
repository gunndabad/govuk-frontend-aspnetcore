using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Generates a GDS details component.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [RestrictChildren(
        DetailsSummaryTagHelper.TagName,
        DetailsSummaryTagHelper.ShortTagName,
        DetailsTextTagHelper.TagName,
        DetailsTextTagHelper.ShortTagName)]
    [OutputElementHint(ComponentGenerator.DetailsElement)]
    public class DetailsTagHelper : TagHelper
    {
        internal const string TagName = "govuk-details";

        private const string OpenAttributeName = "open";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a new <see cref="DetailsTagHelper"/>.
        /// </summary>
        public DetailsTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal DetailsTagHelper(IGovUkHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <summary>
        /// Whether the details element should be expanded.
        /// </summary>
        /// <remarks>
        /// The default is <c>false</c>.
        /// </remarks>
        [HtmlAttributeName(OpenAttributeName)]
        public bool Open { get; set; } = ComponentGenerator.DetailsDefaultOpen;

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var detailsContext = new DetailsContext();

            using (context.SetScopedContextItem(detailsContext))
            {
                await output.GetChildContentAsync();
            }

            detailsContext.ThrowIfNotComplete();

            var tagBuilder = _htmlGenerator.GenerateDetails(
                Open,
                detailsContext.Summary?.Content,
                detailsContext.Summary?.Attributes,
                detailsContext.Text?.Content,
                detailsContext.Text?.Attributes,
                output.Attributes.ToAttributeDictionary());

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
