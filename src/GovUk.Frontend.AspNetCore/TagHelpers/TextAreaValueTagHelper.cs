#nullable enable
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the value of a GDS character count component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TextAreaTagHelper.TagName)]
    public class TextAreaValueTagHelper : TagHelper
    {
        internal const string TagName = "govuk-textarea-value";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            var characterCountContext = context.GetContextItem<TextAreaContext>();
            characterCountContext.SetValue(childContent.Snapshot());

            output.SuppressOutput();
        }
    }
}
