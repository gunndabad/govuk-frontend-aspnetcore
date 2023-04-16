using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the hint of a checkbox item in a GDS checkboxes component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = CheckboxesItemTagHelper.TagName)]
    [HtmlTargetElement(TagName, ParentTag = CheckboxesItemTagHelper.ShortTagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = CheckboxesItemTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = CheckboxesItemTagHelper.ShortTagName)]
    [OutputElementHint(ComponentGenerator.HintElement)]
    public class CheckboxesItemHintTagHelper : TagHelper
    {
        internal const string TagName = "govuk-checkboxes-item-hint";
        internal const string ShortTagName = "hint";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var itemContext = context.GetContextItem<CheckboxesItemContext>();

            var childContent = await output.GetChildContentAsync();

            itemContext.SetHint(output.Attributes.ToAttributeDictionary(), childContent.Snapshot());

            output.SuppressOutput();
        }
    }
}
