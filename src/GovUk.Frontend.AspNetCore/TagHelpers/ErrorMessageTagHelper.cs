using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-error-message", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class ErrorMessageTagHelper : TagHelper
    {
        internal const string DefaultVisuallyHiddenText = "Error";

        private const string AspForAttributeName = "asp-for";
        private const string VisuallyHiddenTextAttributeName = "visually-hidden-text";
        
        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public ErrorMessageTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator;
        }

        [HtmlAttributeName(AspForAttributeName)]
        public ModelExpression AspFor { get; set; }

        [HtmlAttributeName(VisuallyHiddenTextAttributeName)]
        public string VisuallyHiddenText { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            if (!childContent.IsEmptyOrWhiteSpace && AspFor != null)
            {
                throw new InvalidOperationException($"Cannot specify both content and the '{AspForAttributeName}' attribute.");
            }

            var visuallyHiddenText = VisuallyHiddenText ?? DefaultVisuallyHiddenText;

            var tagBuilder = AspFor != null ?
                _htmlGenerator.GenerateErrorMessage(ViewContext, AspFor.ModelExplorer, AspFor.Name, visuallyHiddenText) :
                _htmlGenerator.GenerateErrorMessage(visuallyHiddenText, childContent);

            if (tagBuilder != null)
            {
                output.TagName = tagBuilder.TagName;
                output.TagMode = TagMode.StartTagAndEndTag;

                output.MergeAttributes(tagBuilder);
                output.Content.SetHtmlContent(tagBuilder.InnerHtml);
            }
            else
            {
                output.SuppressOutput();
            }
        }
    }
}
