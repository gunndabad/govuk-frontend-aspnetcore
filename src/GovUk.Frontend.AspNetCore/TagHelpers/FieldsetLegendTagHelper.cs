#nullable enable
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the legend in a GDS fieldset component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = FieldsetTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.FieldsetLegendElement)]
    public class FieldsetLegendTagHelper : TagHelper
    {
        internal const string TagName = "govuk-fieldset-legend";

        private const string IsPageHeadingAttributeName = "is-page-heading";

        /// <summary>
        /// Creates a <see cref="FieldsetLegendTagHelper"/>.
        /// </summary>
        public FieldsetLegendTagHelper()
        {
        }

        /// <summary>
        /// Whether the legend also acts as the heading for the page.
        /// </summary>
        [HtmlAttributeName(IsPageHeadingAttributeName)]
        public bool IsPageHeading { get; set; } = ComponentGenerator.FieldsetLegendDefaultIsPageHeading;

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fieldsetContext = context.GetContextItem<FieldsetContext>();

            var childContent = await output.GetChildContentAsync();

            fieldsetContext.SetLegend(
                IsPageHeading,
                output.Attributes.ToAttributesDictionary(),
                childContent.Snapshot());

            output.SuppressOutput();
        }
    }
}
