using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-phase-banner", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PhaseBannerTagHelper : TagHelper
    {
        private const string _tagAttributeName = "tag";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public PhaseBannerTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        [HtmlAttributeName(_tagAttributeName)]
        public string Tag { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(Tag))
            {
                throw new InvalidOperationException($"You must specify a value for the '{_tagAttributeName}' attribute.");
            }

            var childContent = await output.GetChildContentAsync();

            var tagBuilder = _htmlGenerator.GeneratePhaseBanner(Tag, childContent);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
