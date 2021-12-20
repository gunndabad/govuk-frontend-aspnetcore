#nullable enable
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the conditional reveal of a checkbox item in a GDS checkboxes component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = CheckboxesItemTagHelper.TagName)]
    public class CheckboxesItemConditionalTagHelper : TagHelper
    {
        internal const string TagName = "govuk-checkboxes-item-conditional";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var itemContext = context.GetContextItem<CheckboxesItemContext>();

            var childContent = await output.GetChildContentAsync();

            itemContext.SetConditional(output.Attributes.ToAttributesDictionary(), childContent.Snapshot());

            output.SuppressOutput();
        }
    }
}
