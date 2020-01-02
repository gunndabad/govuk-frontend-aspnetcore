using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-error-message", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class ErrorMessageTagHelper : TagHelper
    {
        private const string AspForAttributeName = "asp-for";
        private const string IdAttibuteName = "id";
        private const string VisuallyHiddenTextAttributeName = "visually-hidden-text";
        
        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public ErrorMessageTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        [HtmlAttributeName(AspForAttributeName)]
        public ModelExpression AspFor { get; set; }

        [HtmlAttributeName(IdAttibuteName)]
        public string Id { get; set; }

        [HtmlAttributeName(VisuallyHiddenTextAttributeName)]
        public string VisuallyHiddenText { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = output.TagMode == TagMode.StartTagAndEndTag ? await output.GetChildContentAsync() : null;

            if (childContent == null && AspFor == null)
            {
                throw new InvalidOperationException($"Cannot determine content.");
            }

            var resolvedContent = (IHtmlContent)childContent;
            if (resolvedContent == null && AspFor != null)
            {
                var validationMessage = _htmlGenerator.GetValidationMessage(ViewContext, AspFor.ModelExplorer, AspFor.Name);

                if (validationMessage != null)
                {
                    resolvedContent = new HtmlString(validationMessage);
                }
            }

            if (resolvedContent != null)
            {
                var tagBuilder = _htmlGenerator.GenerateErrorMessage(VisuallyHiddenText, Id, resolvedContent);

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
