using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the error message in a GDS form group component.
    /// </summary>
    [HtmlTargetElement(CheckboxesTagHelper.ErrorMessageTagName, ParentTag = CheckboxesTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = CheckboxesTagHelper.TagName)]
    [HtmlTargetElement(CheckboxesTagHelper.ErrorMessageTagName, ParentTag = CheckboxesFieldsetTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = CheckboxesFieldsetTagHelper.TagName)]
    [HtmlTargetElement(CheckboxesTagHelper.ErrorMessageTagName, ParentTag = CheckboxesFieldsetTagHelper.ShortTagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = CheckboxesFieldsetTagHelper.ShortTagName)]
    [HtmlTargetElement(CharacterCountTagHelper.ErrorMessageTagName, ParentTag = CharacterCountTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = CharacterCountTagHelper.TagName)]
    [HtmlTargetElement(FileUploadTagHelper.ErrorMessageTagName, ParentTag = FileUploadTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = FileUploadTagHelper.TagName)]
    [HtmlTargetElement(RadiosTagHelper.ErrorMessageTagName, ParentTag = RadiosTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = RadiosTagHelper.TagName)]
    [HtmlTargetElement(RadiosTagHelper.ErrorMessageTagName, ParentTag = RadiosFieldsetTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = RadiosFieldsetTagHelper.TagName)]
    [HtmlTargetElement(RadiosTagHelper.ErrorMessageTagName, ParentTag = RadiosFieldsetTagHelper.ShortTagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = RadiosFieldsetTagHelper.ShortTagName)]
    [HtmlTargetElement(SelectTagHelper.ErrorMessageTagName, ParentTag = SelectTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = SelectTagHelper.TagName)]
    [HtmlTargetElement(TextAreaTagHelper.ErrorMessageTagName, ParentTag = TextAreaTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = TextAreaTagHelper.TagName)]
    [HtmlTargetElement(TextInputTagHelper.ErrorMessageTagName, ParentTag = TextInputTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = TextInputTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.ErrorMessageElement)]
    public class FormGroupErrorMessageTagHelper : TagHelper
    {
        internal const string ShortTagName = "error-message";

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

            SetErrorMessage(childContent, context, output);

            output.SuppressOutput();
        }

        private protected virtual void SetErrorMessage(TagHelperContent? childContent, TagHelperContext context, TagHelperOutput output)
        {
            var formGroupContext = context.GetContextItem<FormGroupContext>();

            formGroupContext.SetErrorMessage(
                VisuallyHiddenText,
                output.Attributes.ToAttributeDictionary(),
                childContent?.Snapshot());
        }
    }
}
