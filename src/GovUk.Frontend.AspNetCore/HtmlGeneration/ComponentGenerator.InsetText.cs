using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    internal partial class ComponentGenerator
    {
        internal const string InsetTextElement = "div";

        public TagBuilder GenerateInsetText(
            string? id,
            IHtmlContent content,
            AttributeDictionary? attributes)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            var tagBuilder = new TagBuilder(InsetTextElement);
            tagBuilder.MergeOptionalAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-inset-text");

            if (id != null)
            {
                tagBuilder.Attributes.Add("id", id);
            }

            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }
    }
}
