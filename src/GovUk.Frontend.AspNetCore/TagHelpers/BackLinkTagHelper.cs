using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-back-link", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class BackLinkTagHelper : AnchorTagHelper
    {
        public BackLinkTagHelper(IHtmlGenerator generator)
            : base(generator)
        {
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.TagMode = TagMode.StartTagAndEndTag;

            await base.ProcessAsync(context, output);

            var childContent = await output.GetChildContentAsync();
            if (childContent.IsEmptyOrWhiteSpace)
            {
                output.Content.Append("Back");
            }

            output.AddClass("govuk-back-link", HtmlEncoder.Default);
        }
    }
}