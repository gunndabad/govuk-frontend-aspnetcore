#nullable enable
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the legend in a GDS date input component's fieldset.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = DateInputFieldsetTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.FieldsetLegendElement)]
    public class DateInputFieldsetLegendTagHelper : TagHelper
    {
        internal const string TagName = "govuk-date-input-fieldset-legend";

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

            var content = await output.GetChildContentAsync();

            fieldsetContext.SetLegend(IsPageHeading, output.Attributes.ToAttributeDictionary(), content: content.Snapshot());

            output.SuppressOutput();
        }
    }
}
