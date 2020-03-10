using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-label", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class LabelTagHelper : TagHelper
    {
        private const string IsPageHeadingAttributeName = "is-page-heading";
        private const string AspForAttributeName = "asp-for";
        private const string ForAttributeName = "for";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public LabelTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        [HtmlAttributeName(AspForAttributeName)]
        public ModelExpression AspFor { get; set; }

        [HtmlAttributeName(ForAttributeName)]
        public string For { get; set; }

        [HtmlAttributeName(IsPageHeadingAttributeName)]
        public bool IsPageHeading { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (For == null && AspFor == null)
            {
                throw new InvalidOperationException($"Cannot determine 'for' attribute for <label>.");
            }

            var childContent = output.TagMode == TagMode.StartTagAndEndTag ? await output.GetChildContentAsync() : null;

            var resolvedFor = For ??
                TagBuilder.CreateSanitizedId(_htmlGenerator.GetFullHtmlFieldName(ViewContext, AspFor.Name), Constants.IdAttributeDotReplacement);

            var resolvedContent = (IHtmlContent)childContent ??
                new HtmlString(_htmlGenerator.GetDisplayName(ViewContext, AspFor.ModelExplorer, AspFor.Name));

            var tagBuilder = _htmlGenerator.GenerateLabel(resolvedFor, IsPageHeading, resolvedContent);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
