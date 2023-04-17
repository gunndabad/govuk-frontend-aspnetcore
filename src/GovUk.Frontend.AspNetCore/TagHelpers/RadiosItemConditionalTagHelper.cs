using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the conditional reveal of a radios item in a GDS radios component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = RadiosItemTagHelper.TagName)]
    [HtmlTargetElement(TagName, ParentTag = RadiosItemTagHelper.ShortTagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = RadiosItemTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = RadiosItemTagHelper.ShortTagName)]
    public class RadiosItemConditionalTagHelper : TagHelper
    {
        internal const string TagName = "govuk-radios-item-conditional";
        internal const string ShortTagName = "conditional";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var itemContext = context.GetContextItem<RadiosItemContext>();

            var childContent = await output.GetChildContentAsync();

            itemContext.SetConditional(output.Attributes.ToAttributeDictionary(), childContent.Snapshot());

            output.SuppressOutput();
        }
    }
}
