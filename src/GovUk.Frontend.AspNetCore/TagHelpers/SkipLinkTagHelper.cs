using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-skip-link", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class SkipLinkTagHelper : LinkTagHelperBase
    {
        public SkipLinkTagHelper(IGovUkHtmlGenerator generator, IUrlHelperFactory urlHelperFactory)
            : base(generator, urlHelperFactory)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var tagBuilder = CreateAnchorTagBuilder();

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.AddClass("govuk-skip-link", HtmlEncoder.Default);
        }
    }
}
