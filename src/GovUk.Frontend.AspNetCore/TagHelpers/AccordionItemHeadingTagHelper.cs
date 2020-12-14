#nullable enable
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = AccordionItemTagHelper.TagName)]
    public class AccordionItemHeadingTagHelper : TagHelper
    {
        internal const string TagName = "govuk-accordion-item-heading";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var itemContext = context.GetContextItem<AccordionItemContext>();

            var childContent = await output.GetChildContentAsync();

            itemContext.SetHeading(output.Attributes.ToAttributesDictionary(), childContent.Snapshot());

            output.SuppressOutput();
        }
    }
}
