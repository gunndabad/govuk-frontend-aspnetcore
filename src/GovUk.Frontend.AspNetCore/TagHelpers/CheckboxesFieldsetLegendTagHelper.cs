using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the legend in a GDS checkboxes component fieldset.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = CheckboxesFieldsetTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = CheckboxesFieldsetTagHelper.TagName)]
    [HtmlTargetElement(TagName, ParentTag = CheckboxesFieldsetTagHelper.ShortTagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = CheckboxesFieldsetTagHelper.ShortTagName)]
    [OutputElementHint(ComponentGenerator.FieldsetLegendElement)]
    public class CheckboxesFieldsetLegendTagHelper : TagHelper
    {
        internal const string TagName = "govuk-checkboxes-fieldset-legend";
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
            var fieldsetContext = context.GetContextItem<CheckboxesFieldsetContext>();

            var content = output.TagMode == TagMode.StartTagAndEndTag ?
                (await output.GetChildContentAsync()).Snapshot() :
                null;

            fieldsetContext.SetLegend(IsPageHeading, output.Attributes.ToAttributeDictionary(), content: content);

            output.SuppressOutput();
        }
    }
}
