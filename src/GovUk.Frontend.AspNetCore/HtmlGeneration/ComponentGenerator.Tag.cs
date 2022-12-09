using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string TagElement = "strong";

        public TagBuilder GenerateTag(
            IHtmlContent content,
            AttributeDictionary? attributes)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            var tagBuilder = new TagBuilder(TagElement);
            tagBuilder.MergeOptionalAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-tag");
            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }
    }
}
