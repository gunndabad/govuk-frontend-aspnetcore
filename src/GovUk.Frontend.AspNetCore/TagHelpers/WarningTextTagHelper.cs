using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-warning-text", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class WarningTextTagHelper : TagHelper
    {
        private const string AttributesPrefix = "warning-text-";
        private const string IconFallbackTextAttributeName = "icon-fallback-text";
        
        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public WarningTextTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        [HtmlAttributeName(IconFallbackTextAttributeName)]
        public string IconFallbackText { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.ThrowIfOutputHasAttributes();

            if (IconFallbackText == null)
            {
                throw new InvalidOperationException(
                    $"You must specify a value for the '{IconFallbackTextAttributeName}' attribute.");
            }

            var childContent = await output.GetChildContentAsync();

            var tagBuilder = _htmlGenerator.GenerateWarningText(Attributes, childContent, IconFallbackText);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}