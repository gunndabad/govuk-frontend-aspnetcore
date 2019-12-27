using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-skip-link", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class SkipLinkTagHelper : LinkTagHelperBase
    {
        public SkipLinkTagHelper(IGovUkHtmlGenerator generator)
            : base(generator)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var tagBuilder = CreateAnchorTagBuilder();

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.MergeAttributes(tagBuilder);
            output.AddClass("govuk-skip-link", HtmlEncoder.Default);
        }
    }
}
