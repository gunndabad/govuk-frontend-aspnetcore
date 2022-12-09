using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Generates a GDS skip link component.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.SkipLinkElement)]
    public class SkipLinkTagHelper : TagHelper
    {
        internal const string TagName = "govuk-skip-link";

        private const string HrefAttributeName = "href";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        private string _href = ComponentGenerator.SkipLinkDefaultHref;

        /// <summary>
        /// Creates a new <see cref="BackLinkTagHelper"/>.
        /// </summary>
        public SkipLinkTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal SkipLinkTagHelper(IGovUkHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <summary>
        /// The <c>href</c> attribute for the link.
        /// </summary>
        /// <remarks>
        /// The default is <c>&quot;#content&quot;</c>.
        /// Cannot be <c>null</c> or empty.
        /// </remarks>
        [HtmlAttributeName(HrefAttributeName)]
        public string Href
        {
            get => _href;
            set => _href = Guard.ArgumentNotNullOrEmpty(nameof(value), value);
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            var tagBuilder = _htmlGenerator.GenerateSkipLink(Href, childContent, output.Attributes.ToAttributeDictionary());

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
