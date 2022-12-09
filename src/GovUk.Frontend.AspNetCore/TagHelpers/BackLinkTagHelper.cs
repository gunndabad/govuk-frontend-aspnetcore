using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Generates a GDS back link component.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.BackLinkElement)]
    public class BackLinkTagHelper : TagHelper
    {
        internal const string TagName = "govuk-back-link";

        private static readonly HtmlString _defaultContent = new HtmlString(ComponentGenerator.BackLinkDefaultContent);

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a new <see cref="BackLinkTagHelper"/>.
        /// </summary>
        public BackLinkTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal BackLinkTagHelper(IGovUkHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            IHtmlContent content = _defaultContent;

            if (output.TagMode == TagMode.StartTagAndEndTag)
            {
                content = await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateBackLink(content, output.Attributes.ToAttributeDictionary());

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
