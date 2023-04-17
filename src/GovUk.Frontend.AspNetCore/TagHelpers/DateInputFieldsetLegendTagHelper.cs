using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the legend in a GDS date input component's fieldset.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = DateInputFieldsetTagHelper.TagName)]
    [HtmlTargetElement(TagName, ParentTag = DateInputFieldsetTagHelper.ShortTagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = DateInputFieldsetTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = DateInputFieldsetTagHelper.ShortTagName)]
    [OutputElementHint(ComponentGenerator.FieldsetLegendElement)]
    public class DateInputFieldsetLegendTagHelper : TagHelper
    {
        internal const string TagName = "govuk-date-input-fieldset-legend";
        internal const string ShortTagName = "legend";

        private const string IsPageHeadingAttributeName = "is-page-heading";

        /// <summary>
        /// Whether the legend also acts as the heading for the page.
        /// </summary>
        /// <remarks>
        /// Defaults to <c>false</c>.
        /// </remarks>
        [HtmlAttributeName(IsPageHeadingAttributeName)]
        public bool IsPageHeading { get; set; } = ComponentGenerator.FieldsetLegendDefaultIsPageHeading;

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fieldsetContext = context.GetContextItem<DateInputFieldsetContext>();

            var content = output.TagMode == TagMode.StartTagAndEndTag ?
                (await output.GetChildContentAsync()).Snapshot() :
                null;

            fieldsetContext.SetLegend(IsPageHeading, output.Attributes.ToAttributeDictionary(), content: content);

            output.SuppressOutput();
        }
    }
}
