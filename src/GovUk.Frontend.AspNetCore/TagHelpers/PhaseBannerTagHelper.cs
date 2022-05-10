#nullable enable
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Generates a GDS phase banner component.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.PhaseBannerElement)]
    public class PhaseBannerTagHelper : TagHelper
    {
        internal const string TagName = "govuk-phase-banner";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a <see cref="PhaseBannerTagHelper"/>.
        /// </summary>
        public PhaseBannerTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal PhaseBannerTagHelper(IGovUkHtmlGenerator? htmlGenerator = null)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var phaseBannerContext = new PhaseBannerContext();

            IHtmlContent childContent;

            using (context.SetScopedContextItem(phaseBannerContext))
            {
                childContent = await output.GetChildContentAsync();
            }

            phaseBannerContext.ThrowIfIncomplete();

            var tagBuilder = _htmlGenerator.GeneratePhaseBanner(
                phaseBannerContext.Tag?.Content,
                phaseBannerContext.Tag?.Attributes,
                childContent,
                output.Attributes.ToAttributeDictionary());

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
