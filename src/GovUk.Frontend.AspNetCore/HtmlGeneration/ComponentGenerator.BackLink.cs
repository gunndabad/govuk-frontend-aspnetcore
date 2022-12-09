using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string BackLinkDefaultContent = "Back";
        internal const string BackLinkElement = "a";

        public virtual TagBuilder GenerateBackLink(
            IHtmlContent content,
            AttributeDictionary? attributes)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            var tagBuilder = new TagBuilder(BackLinkElement);
            tagBuilder.MergeOptionalAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-back-link");
            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }
    }
}
