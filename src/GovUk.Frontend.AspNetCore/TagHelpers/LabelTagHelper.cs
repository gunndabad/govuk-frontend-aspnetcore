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
        private readonly IHtmlHelper _htmlHelper;

        public LabelTagHelper(IGovUkHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
            _htmlHelper = htmlHelper ?? throw new ArgumentNullException(nameof(htmlHelper));
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
            (_htmlHelper as IViewContextAware)?.Contextualize(ViewContext);

            if (For == null && AspFor == null)
            {
                throw new InvalidOperationException($"Cannot determine 'for' attribute for <label>.");
            }

            var childContent = await output.GetChildContentAsync();

            var @for = For ?? _htmlHelper.Id(AspFor.Name);

            var content = childContent.IsEmptyOrWhiteSpace && AspFor != null ?
                new StringHtmlContent(_htmlHelper.DisplayName(AspFor.Name)) :
                (IHtmlContent)childContent;

            var tagBuilder = _htmlGenerator.GenerateLabel(@for, IsPageHeading, content);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
