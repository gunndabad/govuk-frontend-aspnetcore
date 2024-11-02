using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration;

internal partial class ComponentGenerator
{
    internal const int PanelDefaultHeadingLevel = 1;
    internal const string PanelElement = "div";
    internal const int PanelMinHeadingLevel = 1;
    internal const int PanelMaxHeadingLevel = 6;

    public TagBuilder GeneratePanel(
        int headingLevel,
        IHtmlContent titleContent,
        IHtmlContent? bodyContent,
        AttributeDictionary? attributes
    )
    {
        if (headingLevel < PanelMinHeadingLevel || headingLevel > PanelMaxHeadingLevel)
        {
            throw new ArgumentOutOfRangeException(
                $"{nameof(headingLevel)} must be between {PanelMinHeadingLevel} and {PanelMaxHeadingLevel}.",
                nameof(headingLevel)
            );
        }

        Guard.ArgumentNotNull(nameof(titleContent), titleContent);

        var tagBuilder = new TagBuilder(PanelElement);
        tagBuilder.MergeOptionalAttributes(attributes);
        tagBuilder.MergeCssClass("govuk-panel");
        tagBuilder.MergeCssClass("govuk-panel--confirmation");

        var heading = new TagBuilder($"h{headingLevel}");
        heading.MergeCssClass("govuk-panel__title");
        heading.InnerHtml.AppendHtml(titleContent);
        tagBuilder.InnerHtml.AppendHtml(heading);

        if (bodyContent != null)
        {
            var body = new TagBuilder("div");
            body.MergeCssClass("govuk-panel__body");
            body.InnerHtml.AppendHtml(bodyContent);
            tagBuilder.InnerHtml.AppendHtml(body);
        }

        return tagBuilder;
    }
}
