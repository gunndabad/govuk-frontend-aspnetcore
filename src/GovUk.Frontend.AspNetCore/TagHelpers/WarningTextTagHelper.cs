using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-warning-text", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class WarningTextTagHelper : TagHelper
    {
        private const string IconFallbackTextAttributeName = "icon-fallback-text";

        [HtmlAttributeName(IconFallbackTextAttributeName)]
        public string IconFallbackText { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (IconFallbackText == null)
            {
                throw new InvalidOperationException($"You must specify a value for the '{IconFallbackTextAttributeName}' attribute.");
            }

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.AddClass("govuk-warning-text", HtmlEncoder.Default);

            var icon = new TagBuilder("span");
            icon.AddCssClass("govuk-warning-text__icon");
            icon.Attributes.Add("aria-hidden", "true");
            icon.InnerHtml.Append("!");

            output.Content.AppendHtml(icon);

            var text = new TagBuilder("strong");
            text.AddCssClass("govuk-warning-text__text");

            var iconFallback = new TagBuilder("span");
            iconFallback.AddCssClass("govuk-warning-text__assistive");
            iconFallback.InnerHtml.Append(IconFallbackText);

            text.InnerHtml.AppendHtml(iconFallback);

            var childContent = await output.GetChildContentAsync();
            text.InnerHtml.AppendHtml(childContent);

            output.Content.AppendHtml(text);
        }
    }
}