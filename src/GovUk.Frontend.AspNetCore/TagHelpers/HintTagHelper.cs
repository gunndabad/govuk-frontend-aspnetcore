using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-hint", TagStructure = TagStructure.Unspecified)]
    public class HintTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.AddClass("govuk-hint", HtmlEncoder.Default);

            var childContent = await output.GetChildContentAsync();
            output.Content.AppendHtml(childContent);
        }
    }
}