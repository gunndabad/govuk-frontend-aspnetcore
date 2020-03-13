using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-phase-banner", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PhaseBannerTagHelper : TagHelper
    {
        private const string AttributesPrefix = "phase-banner-";
        private const string TagAttributeName = "tag";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public PhaseBannerTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; }

        [HtmlAttributeName(TagAttributeName)]
        public string Tag { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.ThrowIfOutputHasAttributes();

            if (string.IsNullOrEmpty(Tag))
            {
                throw new InvalidOperationException($"You must specify a value for the '{TagAttributeName}' attribute.");
            }

            var childContent = await output.GetChildContentAsync();

            var tagBuilder = _htmlGenerator.GeneratePhaseBanner(Tag, Attributes, childContent);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
