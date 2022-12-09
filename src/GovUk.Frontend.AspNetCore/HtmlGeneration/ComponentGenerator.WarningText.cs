using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    internal partial class ComponentGenerator
    {
        internal const string WarningTextElement = "div";

        public TagBuilder GenerateWarningText(
            string iconFallbackText,
            IHtmlContent content,
            AttributeDictionary? attributes)
        {
            Guard.ArgumentNotNull(nameof(iconFallbackText), iconFallbackText);
            Guard.ArgumentNotNull(nameof(content), content);

            var tagBuilder = new TagBuilder(WarningTextElement);
            tagBuilder.MergeOptionalAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-warning-text");

            var icon = new TagBuilder("span");
            icon.MergeCssClass("govuk-warning-text__icon");
            icon.Attributes.Add("aria-hidden", "true");
            icon.InnerHtml.Append("!");

            tagBuilder.InnerHtml.AppendHtml(icon);

            var text = new TagBuilder("strong");
            text.MergeCssClass("govuk-warning-text__text");

            var iconFallback = new TagBuilder("span");
            iconFallback.MergeCssClass("govuk-warning-text__assistive");
            iconFallback.InnerHtml.Append(iconFallbackText);

            text.InnerHtml.AppendHtml(iconFallback);

            text.InnerHtml.AppendHtml(content);

            tagBuilder.InnerHtml.AppendHtml(text);

            return tagBuilder;
        }
    }
}
