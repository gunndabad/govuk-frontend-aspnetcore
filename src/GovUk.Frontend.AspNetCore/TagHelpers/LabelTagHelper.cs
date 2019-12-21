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
    [HtmlTargetElement("govuk-label", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class LabelTagHelper : TagHelper
    {
        private const string IsPageHeadingAttributeName = "is-page-heading";
        private const string AspForAttributeName = "asp-for";
        private const string ForAttributeName = "for";

        private readonly IHtmlGenerator _htmlGenerator;

        public LabelTagHelper(IHtmlGenerator htmlGenerator)
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

            output.TagName = "label";
            output.TagMode = TagMode.StartTagAndEndTag;

            var childContent = await output.GetChildContentAsync();

            IHtmlContent content = childContent;
            string @for = For;

            if (AspFor != null)
            {
                var label = _htmlGenerator.GenerateLabel(
                    ViewContext,
                    AspFor.ModelExplorer,
                    AspFor.Name,
                    labelText: null,
                    htmlAttributes: null);

                if (childContent.IsEmptyOrWhiteSpace)
                {
                    content = label.InnerHtml;
                }

                if (For == null)
                {
                    @for = label.Attributes["for"];
                }
            }

            output.AddClass("govuk-label", HtmlEncoder.Default);
            output.Attributes.Add("for", @for);

            if (IsPageHeading)
            {
                var header = new TagBuilder("h1");
                header.AddCssClass("govuk-label-wrapper");

                output.PreElement.AppendHtml(header.RenderStartTag());
                output.PostElement.AppendHtml(header.RenderEndTag());
            }

            output.Content.AppendHtml(content);
        }
    }
}
