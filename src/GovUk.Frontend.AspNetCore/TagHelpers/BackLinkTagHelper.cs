#nullable enable
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Generates a GDS back link component.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.BackLinkElement)]
    public class BackLinkTagHelper : LinkTagHelperBase
    {
        internal const string TagName = "govuk-back-link";

        private static readonly HtmlString _defaultContent = new HtmlString(ComponentGenerator.BackLinkDefaultContent);

        /// <summary>
        /// Creates a new <see cref="BackLinkTagHelper"/>.
        /// </summary>
        /// <param name="urlHelperFactory">The <see cref="IUrlHelperFactory"/>.</param>
        public BackLinkTagHelper(IUrlHelperFactory urlHelperFactory)
            : this(htmlGenerator: null, urlHelperFactory)
        {
        }

        internal BackLinkTagHelper(
            IGovUkHtmlGenerator? htmlGenerator,
            IUrlHelperFactory urlHelperFactory)
            : base(htmlGenerator ?? new ComponentGenerator(), urlHelperFactory)
        {
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            IHtmlContent content = _defaultContent;

            if (output.TagMode == TagMode.StartTagAndEndTag)
            {
                content = await output.GetChildContentAsync();
            }

            var href = ResolveHref();

            var tagBuilder = Generator.GenerateBackLink(href, content, output.Attributes.ToAttributesDictionary());

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
