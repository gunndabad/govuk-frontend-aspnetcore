using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration;

internal partial class ComponentGenerator
{
    internal const string LabelElement = "label";
    internal const bool LabelDefaultIsPageHeading = false;

    [return: NotNullIfNotNull(nameof(content))]
    public TagBuilder? GenerateLabel(
        string? @for,
        bool isPageHeading,
        IHtmlContent? content,
        AttributeDictionary? attributes
    )
    {
        TagBuilder? tagBuilder = null;

        if (content != null)
        {
            tagBuilder = new TagBuilder(LabelElement);
            tagBuilder.MergeOptionalAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-label");

            if (@for != null)
            {
                tagBuilder.Attributes.Add("for", @for);
            }

            tagBuilder.InnerHtml.AppendHtml(content);

            if (isPageHeading)
            {
                var heading = new TagBuilder("h1");
                heading.MergeCssClass("govuk-label-wrapper");

                heading.InnerHtml.AppendHtml(tagBuilder);

                return heading;
            }
        }

        return tagBuilder;
    }
}
