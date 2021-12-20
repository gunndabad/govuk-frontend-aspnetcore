#nullable enable
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the hint in a GDS form group component.
    /// </summary>
    [HtmlTargetElement(CheckboxesTagHelper.HintTagName, ParentTag = CheckboxesTagHelper.TagName)]
    [HtmlTargetElement(CheckboxesTagHelper.HintTagName, ParentTag = CheckboxesFieldsetTagHelper.TagName)]
    [HtmlTargetElement(FileUploadTagHelper.HintTagName, ParentTag = FileUploadTagHelper.TagName)]
    [HtmlTargetElement(SelectTagHelper.HintTagName, ParentTag = SelectTagHelper.TagName)]
    [HtmlTargetElement(TextInputTagHelper.HintTagName, ParentTag = TextInputTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.HintElement)]
    public class FormGroupHintTagHelper : TagHelper
    {
        /// <summary>
        /// Creates a <see cref="FormGroupHintTagHelper"/>.
        /// </summary>
        public FormGroupHintTagHelper()
        {
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = output.TagMode == TagMode.StartTagAndEndTag ?
                await output.GetChildContentAsync() :
                null;

            var formGroupContext = context.GetContextItem<FormGroupContext>();

            formGroupContext.SetHint(output.Attributes.ToAttributesDictionary(), childContent?.Snapshot());

            output.SuppressOutput();
        }
    }
}
