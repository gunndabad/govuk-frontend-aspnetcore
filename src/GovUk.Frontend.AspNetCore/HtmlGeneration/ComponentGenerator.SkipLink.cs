#nullable enable
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string SkipLinkDefaultHref = "#content";
        internal const string SkipLinkElement = "a";

        public TagBuilder GenerateSkipLink(
            string href,
            IHtmlContent content,
            AttributeDictionary? attributes)
        {
            Guard.ArgumentNotNullOrEmpty(nameof(href), href);
            Guard.ArgumentNotNull(nameof(content), content);

            var tagBuilder = new TagBuilder(SkipLinkElement);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-skip-link");
            tagBuilder.Attributes.Add("href", href);
            tagBuilder.Attributes.Add("data-module", "govuk-skip-link");
            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }
    }
}
