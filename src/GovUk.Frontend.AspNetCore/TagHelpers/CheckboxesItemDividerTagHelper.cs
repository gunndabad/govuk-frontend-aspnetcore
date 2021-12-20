#nullable enable
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the divider text to separate items in a GDS checkboxes component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = CheckboxesTagHelper.TagName)]
    [HtmlTargetElement(TagName, ParentTag = CheckboxesFieldsetTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.CheckboxesDividerItemElement)]
    public class CheckboxesItemDividerTagHelper : TagHelper
    {
        internal const string TagName = "govuk-checkboxes-divider";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var checkboxesContext = context.GetContextItem<CheckboxesContext>();

            var content = await output.GetChildContentAsync();

            checkboxesContext.AddItem(new CheckboxesItemDivider()
            {
                Attributes = output.Attributes.ToAttributesDictionary(),
                Content = content.Snapshot()
            });

            output.SuppressOutput();
        }
    }
}
