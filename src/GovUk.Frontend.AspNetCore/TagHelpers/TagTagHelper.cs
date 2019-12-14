using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-tag", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class TagTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "strong";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.AddClass("govuk-tag", HtmlEncoder.Default);

            var childContent = await output.GetChildContentAsync();
            output.Content.AppendHtml(childContent);
        }
    }
}