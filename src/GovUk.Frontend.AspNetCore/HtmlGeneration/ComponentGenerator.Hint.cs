using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration;

internal partial class ComponentGenerator
{
    internal const string HintElement = "div";

    public TagBuilder GenerateHint(string? id, IHtmlContent content, AttributeDictionary? attributes)
    {
        Guard.ArgumentNotNull(nameof(content), content);

        var tagBuilder = new TagBuilder(HintElement);
        tagBuilder.MergeOptionalAttributes(attributes);
        tagBuilder.MergeCssClass("govuk-hint");

        if (!string.IsNullOrEmpty(id))
        {
            tagBuilder.Attributes.Add("id", id);
        }

        tagBuilder.InnerHtml.AppendHtml(content);

        return tagBuilder;
    }
}
