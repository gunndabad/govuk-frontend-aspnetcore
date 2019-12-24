using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-back-link", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class BackLinkTagHelper : LinkTagHelperBase
    {
        public BackLinkTagHelper(IHtmlGenerator generator)
            : base(generator)
        {
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var tagBuilder = CreateAnchorTagBuilder();

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            var childContent = await output.GetChildContentAsync();
            if (childContent.IsEmptyOrWhiteSpace)
            {
                output.Content.Append("Back");
            }

            output.MergeAttributes(tagBuilder);
            output.AddClass("govuk-back-link", HtmlEncoder.Default);
        }
    }
}