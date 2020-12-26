#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string TagElement = "strong";

        public TagBuilder GenerateTag(
            IHtmlContent content,
            IDictionary<string, string>? attributes)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            var tagBuilder = new TagBuilder(TagElement);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-tag");
            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }
    }
}
