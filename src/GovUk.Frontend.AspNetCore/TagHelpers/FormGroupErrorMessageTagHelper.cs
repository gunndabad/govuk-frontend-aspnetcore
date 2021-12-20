#nullable enable
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the error message in a GDS form group component.
    /// </summary>
    [HtmlTargetElement(CheckboxesTagHelper.ErrorMessageTagName, ParentTag = CheckboxesTagHelper.TagName)]
    [HtmlTargetElement(CheckboxesTagHelper.ErrorMessageTagName, ParentTag = CheckboxesFieldsetTagHelper.TagName)]
    [HtmlTargetElement(FileUploadTagHelper.ErrorMessageTagName, ParentTag = FileUploadTagHelper.TagName)]
    [HtmlTargetElement(SelectTagHelper.ErrorMessageTagName, ParentTag = SelectTagHelper.TagName)]
    [HtmlTargetElement(TextInputTagHelper.ErrorMessageTagName, ParentTag = TextInputTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.ErrorMessageElement)]
    public class FormGroupErrorMessageTagHelper : TagHelper
    {
        private const string VisuallyHiddenTextAttributeName = "visually-hidden-text";

        /// <summary>
        /// Creates a <see cref="FormGroupErrorMessageTagHelper"/>.
        /// </summary>
        public FormGroupErrorMessageTagHelper()
        {
        }

        /// <summary>
        /// A visually hidden prefix used before the error message.
        /// </summary>
        /// <remarks>
        /// Defaults to <c>&quot;Error&quot;</c>.
        /// </remarks>
        [HtmlAttributeName(VisuallyHiddenTextAttributeName)]
        public string? VisuallyHiddenText { get; set; }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = output.TagMode == TagMode.StartTagAndEndTag ?
                await output.GetChildContentAsync() :
                null;

            var formGroupContext = context.GetContextItem<FormGroupContext>();

            formGroupContext.SetErrorMessage(
                VisuallyHiddenText,
                output.Attributes.ToAttributesDictionary(),
                childContent?.Snapshot());

            output.SuppressOutput();
        }
    }
}
