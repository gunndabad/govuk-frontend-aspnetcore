#nullable enable
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Generates a GDS breadcrumbs component.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [RestrictChildren(BreadcrumbsItemTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.BreadcrumbsElement)]
    public class BreadcrumbsTagHelper : TagHelper
    {
        internal const string TagName = "govuk-breadcrumbs";

        private const string CollapseOnMobileAttributeName = "collapse-on-mobile";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a new <see cref="BreadcrumbsTagHelper"/>.
        /// </summary>
        public BreadcrumbsTagHelper()
            : this(null)
        {
        }

        internal BreadcrumbsTagHelper(IGovUkHtmlGenerator? htmlGenerator = null)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <summary>
        /// Whether to collapse to the first and last item only on tablet breakpoint and below.
        /// </summary>
        /// <remarks>
        /// The default is <see langword="false"/>.
        /// </remarks>
        [HtmlAttributeName(CollapseOnMobileAttributeName)]
        public bool CollapseOnMobile { get; set; } = ComponentGenerator.BreadcrumbsDefaultCollapseOnMobile;

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var breadcrumbsContext = new BreadcrumbsContext();

            using (context.SetScopedContextItem(breadcrumbsContext))
            {
                await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateBreadcrumbs(
                CollapseOnMobile,
                output.Attributes.ToAttributesDictionary(),
                breadcrumbsContext.Items);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
