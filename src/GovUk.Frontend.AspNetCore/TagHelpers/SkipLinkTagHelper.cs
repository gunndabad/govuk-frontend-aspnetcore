using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-skip-link", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class SkipLinkTagHelper : AnchorTagHelper
    {
        public SkipLinkTagHelper(IHtmlGenerator generator)
            : base(generator)
        {
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.TagMode = TagMode.StartTagAndEndTag;

            await base.ProcessAsync(context, output);

            output.AddClass("govuk-skip-link", HtmlEncoder.Default);
        }
    }
}
