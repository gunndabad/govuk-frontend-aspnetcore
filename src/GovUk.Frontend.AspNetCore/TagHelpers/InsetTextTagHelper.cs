#nullable enable
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Generates a GDS inset text component.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.InsetTextElement)]
    public class InsetTextTagHelper : TagHelper
    {
        internal const string TagName = "govuk-inset-text";

        private const string IdAttributeName = "id";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a new <see cref="InsetTextTagHelper"/>.
        /// </summary>
        public InsetTextTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal InsetTextTagHelper(IGovUkHtmlGenerator? htmlGenerator = null)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <summary>
        /// The <c>id</c> attribute.
        /// </summary>
        [HtmlAttributeName(IdAttributeName)]
        public string? Id { get; set; }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            var tagBuilder = _htmlGenerator.GenerateInsetText(
                Id,
                childContent,
                output.Attributes.ToAttributesDictionary());

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
