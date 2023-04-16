using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the value of a GDS character count component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = CharacterCountTagHelper.TagName)]
    public class CharacterCountValueTagHelper : TagHelper
    {
        internal const string TagName = "govuk-character-count-value";
        internal const string ShortTagName = "value";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            var characterCountContext = context.GetContextItem<CharacterCountContext>();
            characterCountContext.SetValue(childContent.Snapshot());

            output.SuppressOutput();
        }
    }
}
