using System;
using System.Text.Encodings.Web;
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
        
        private readonly IHtmlGenerator _htmlGenerator;

        public ErrorMessageTagHelper(IHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator;
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
            var childContent = await output.GetChildContentAsync();

            if (!childContent.IsEmptyOrWhiteSpace && AspFor != null)
            {
                throw new InvalidOperationException($"Cannot specify both content and the '{AspForAttributeName}' attribute.");
            }

            IHtmlContent content;
            if (AspFor != null)
            {
                var validationMessage = _htmlGenerator.GenerateValidationMessage(
                    ViewContext,
                    AspFor.ModelExplorer,
                    AspFor.Name,
                    message: null,
                    tag: null,
                    htmlAttributes: null);
                content = validationMessage.InnerHtml;
            }
            else
            {
                content = childContent;
            }

            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.AddClass("govuk-error-message", HtmlEncoder.Default);

            if (!string.IsNullOrEmpty(Id))
            {
                output.Attributes.Add("id", Id);
            }

            var vht = new TagBuilder("span");
            vht.AddCssClass("govuk-visually-hidden");
            vht.InnerHtml.Append(!string.IsNullOrEmpty(VisuallyHiddenText) ? VisuallyHiddenText : "Error");
            output.Content.AppendHtml(vht);

            output.Content.AppendHtml(content);
        }
    }
}
