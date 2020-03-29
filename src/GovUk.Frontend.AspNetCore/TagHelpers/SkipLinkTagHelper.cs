using System.Threading.Tasks;
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

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            var href = ResolveHref();

            var tagBuilder = Generator.GenerateSkipLink(href, childContent, output.Attributes.ToAttributesDictionary());

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
