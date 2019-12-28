using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-back-link", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class BackLinkTagHelper : LinkTagHelperBase
    {
        public BackLinkTagHelper(IGovUkHtmlGenerator htmlGenerator)
            : base(htmlGenerator)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var originalTagMode = output.TagMode;

            var tagBuilder = CreateAnchorTagBuilder();

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            if (originalTagMode == TagMode.SelfClosing)
            {
                output.Content.Append("Back");
            }

            output.MergeAttributes(tagBuilder);
            output.AddClass("govuk-back-link", HtmlEncoder.Default);
        }
    }
}