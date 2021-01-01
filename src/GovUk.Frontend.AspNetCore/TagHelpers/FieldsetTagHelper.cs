#nullable enable
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Generates a GDS fieldset component.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.FieldsetElement)]
    public class FieldsetTagHelper : TagHelper
    {
        internal const string TagName = "govuk-fieldset";

        private const string DescribedByAttributeName = "described-by";
        private const string RoleAttributeName = "role";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a new <see cref="FieldsetTagHelper"/>.
        /// </summary>
        public FieldsetTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal FieldsetTagHelper(IGovUkHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <summary>
        /// One or more element IDs to add to the <c>aria-describedby</c> attribute.
        /// </summary>
        [HtmlAttributeName(DescribedByAttributeName)]
        public string? DescribedBy { get; set; }

        /// <summary>
        /// The <c>role</c> attribute.
        /// </summary>
        [HtmlAttributeName(RoleAttributeName)]
        public string? Role { get; set; }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fieldsetContext = new FieldsetContext();

            IHtmlContent childContent;

            using (context.SetScopedContextItem(fieldsetContext))
            {
                childContent = await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateFieldset(
                DescribedBy,
                Role,
                fieldsetContext.Legend?.IsPageHeading,
                fieldsetContext.Legend?.Content,
                fieldsetContext.Legend?.Attributes,
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
