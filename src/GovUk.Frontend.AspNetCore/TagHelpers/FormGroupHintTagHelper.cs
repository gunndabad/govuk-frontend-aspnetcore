using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the hint in a GDS form group component.
    /// </summary>
    [HtmlTargetElement(CheckboxesTagHelper.HintTagName, ParentTag = CheckboxesTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = CheckboxesTagHelper.TagName)]
    [HtmlTargetElement(CheckboxesTagHelper.HintTagName, ParentTag = CheckboxesFieldsetTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = CheckboxesFieldsetTagHelper.TagName)]
    [HtmlTargetElement(CheckboxesTagHelper.HintTagName, ParentTag = CheckboxesFieldsetTagHelper.ShortTagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = CheckboxesFieldsetTagHelper.ShortTagName)]
    [HtmlTargetElement(CharacterCountTagHelper.HintTagName, ParentTag = CharacterCountTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = CharacterCountTagHelper.TagName)]
    [HtmlTargetElement(DateInputTagHelper.HintTagName, ParentTag = DateInputTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = DateInputTagHelper.TagName)]
    [HtmlTargetElement(DateInputTagHelper.HintTagName, ParentTag = DateInputFieldsetTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = DateInputFieldsetTagHelper.TagName)]
    [HtmlTargetElement(DateInputTagHelper.HintTagName, ParentTag = DateInputFieldsetTagHelper.ShortTagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = DateInputFieldsetTagHelper.ShortTagName)]
    [HtmlTargetElement(FileUploadTagHelper.HintTagName, ParentTag = FileUploadTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = FileUploadTagHelper.TagName)]
    [HtmlTargetElement(RadiosTagHelper.HintTagName, ParentTag = RadiosTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = RadiosTagHelper.TagName)]
    [HtmlTargetElement(RadiosTagHelper.HintTagName, ParentTag = RadiosFieldsetTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = RadiosFieldsetTagHelper.TagName)]
    [HtmlTargetElement(RadiosTagHelper.HintTagName, ParentTag = RadiosFieldsetTagHelper.ShortTagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = RadiosFieldsetTagHelper.ShortTagName)]
    [HtmlTargetElement(SelectTagHelper.HintTagName, ParentTag = SelectTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = SelectTagHelper.TagName)]
    [HtmlTargetElement(TextAreaTagHelper.HintTagName, ParentTag = TextAreaTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = TextAreaTagHelper.TagName)]
    [HtmlTargetElement(TextInputTagHelper.HintTagName, ParentTag = TextInputTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = TextInputTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.HintElement)]
    public class FormGroupHintTagHelper : TagHelper
    {
        internal const string ShortTagName = "hint";

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

            formGroupContext.SetHint(output.Attributes.ToAttributeDictionary(), childContent?.Snapshot());

            output.SuppressOutput();
        }
    }
}
