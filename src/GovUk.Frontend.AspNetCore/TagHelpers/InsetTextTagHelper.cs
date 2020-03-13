using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-inset-text", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class InsetTextTagHelper : TagHelper
    {
        private const string AttributesPrefix = "inset-text-*";
        private const string IdAttributeName = "id";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public InsetTextTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; }

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.ThrowIfOutputHasAttributes();

            var childContent = await output.GetChildContentAsync();

            var tagBuilder = _htmlGenerator.GenerateInsetText(Id, Attributes, childContent);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
