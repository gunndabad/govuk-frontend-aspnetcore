using System;
using System.Threading.Tasks;
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
            _htmlGenerator = htmlGenerator;
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
            if (AspFor != null && For != null)
            {
                throw new InvalidOperationException($"Cannot specify both the '{AspForAttributeName}' and '{ForAttributeName}' attributes.");
            }

            var childContent = await output.GetChildContentAsync();
            var content = childContent.IsEmptyOrWhiteSpace ? null : childContent;

            var tagBuilder = AspFor != null ?
                _htmlGenerator.GenerateLabel(ViewContext, AspFor.ModelExplorer, AspFor.Name, IsPageHeading, content) :
                _htmlGenerator.GenerateLabel(For, IsPageHeading, content);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
