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
        private const string AspForAttributeName = "asp-for";
        private const string IdAttibuteName = "id";
        private const string VisuallyHiddenTextAttributeName = "visually-hidden-text";
        
        private readonly IGovUkHtmlGenerator _htmlGenerator;
        private readonly IHtmlHelper _htmlHelper;

        public ErrorMessageTagHelper(IGovUkHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
            _htmlHelper = htmlHelper ?? throw new ArgumentNullException(nameof(htmlHelper));
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
            (_htmlHelper as IViewContextAware)?.Contextualize(ViewContext);

            var childContent = await output.GetChildContentAsync();

            var content = childContent.IsEmptyOrWhiteSpace && AspFor != null ?
                _htmlHelper.ValidationMessage(AspFor.Name) :
                childContent;

            var visuallyHiddenText = VisuallyHiddenText ?? "Error";

            var tagBuilder = _htmlGenerator.GenerateErrorMessage(visuallyHiddenText, content);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            if (!string.IsNullOrEmpty(Id))
            {
                output.Attributes.Add("id", Id);
            }

            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
