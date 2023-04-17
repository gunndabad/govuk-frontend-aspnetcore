using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the legend in a GDS radios component fieldset.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = RadiosFieldsetTagHelper.TagName)]
    [HtmlTargetElement(TagName, ParentTag = RadiosFieldsetTagHelper.ShortTagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = RadiosFieldsetTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = RadiosFieldsetTagHelper.ShortTagName)]
    [OutputElementHint(ComponentGenerator.FieldsetLegendElement)]
    public class RadiosFieldsetLegendTagHelper : TagHelper
    {
        internal const string TagName = "govuk-radios-fieldset-legend";
        internal const string ShortTagName = "legend";

        private const string IsPageHeadingAttributeName = "is-page-heading";

        /// <summary>
        /// Whether the legend also acts as the heading for the page.
        /// </summary>
        /// <remarks>
        /// The default is <c>false</c>.
        /// </remarks>
        [HtmlAttributeName(IsPageHeadingAttributeName)]
        public bool IsPageHeading { get; set; } = ComponentGenerator.FieldsetLegendDefaultIsPageHeading;

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fieldsetContext = context.GetContextItem<RadiosFieldsetContext>();

            var content = output.TagMode == TagMode.StartTagAndEndTag ?
                (await output.GetChildContentAsync()).Snapshot() :
                null;

            fieldsetContext.SetLegend(IsPageHeading, output.Attributes.ToAttributeDictionary(), content: content);

            output.SuppressOutput();
        }
    }
}
