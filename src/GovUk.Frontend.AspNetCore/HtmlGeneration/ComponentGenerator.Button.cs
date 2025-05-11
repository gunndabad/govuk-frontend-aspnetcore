using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration;

internal partial class ComponentGenerator
{
    internal const bool ButtonDefaultDisabled = false;
    internal const bool ButtonDefaultIsStartButton = false;
    internal const string ButtonElement = "button";
    internal const string ButtonLinkElement = "a";

    public TagBuilder GenerateButton(
        bool isStartButton,
        bool disabled,
        bool? preventDoubleClick,
        string? id,
        IHtmlContent content,
        AttributeDictionary? attributes)
    {
        Guard.ArgumentNotNull(nameof(content), content);

        var tagBuilder = new TagBuilder(ButtonElement);
        tagBuilder.MergeOptionalAttributes(attributes);
        tagBuilder.MergeCssClass("govuk-button");
        tagBuilder.Attributes.Add("data-module", "govuk-button");

        if (disabled)
        {
            tagBuilder.Attributes.Add("disabled", "disabled");
            tagBuilder.Attributes.Add("aria-disabled", "true");
        }

        if (preventDoubleClick.HasValue)
        {
            tagBuilder.Attributes.Add("data-prevent-double-click", preventDoubleClick.Value ? "true" : "false");
        }

        if (id != null)
        {
            tagBuilder.Attributes.Add("id", id);
        }

        tagBuilder.InnerHtml.AppendHtml(content);

        if (isStartButton)
        {
            tagBuilder.MergeCssClass("govuk-button--start");

            var icon = GenerateStartButton();
            tagBuilder.InnerHtml.AppendHtml(icon);
        }

        return tagBuilder;
    }

    public TagBuilder GenerateButtonLink(
        bool isStartButton,
        bool disabled,
        string? id,
        IHtmlContent content,
        AttributeDictionary attributes)
    {
        Guard.ArgumentNotNull(nameof(content), content);

        var tagBuilder = new TagBuilder(ButtonLinkElement);
        tagBuilder.MergeAttributes(attributes);
        tagBuilder.MergeCssClass("govuk-button");
        tagBuilder.Attributes.Add("data-module", "govuk-button");
        tagBuilder.Attributes.Add("role", "button");
        tagBuilder.Attributes.Add("draggable", "false");

        if (disabled)
        {
            tagBuilder.MergeCssClass("govuk-button--disabled");
        }

        if (id != null)
        {
            tagBuilder.Attributes.Add("id", id);
        }

        tagBuilder.InnerHtml.AppendHtml(content);

        if (isStartButton)
        {
            tagBuilder.MergeCssClass("govuk-button--start");

            var icon = GenerateStartButton();
            tagBuilder.InnerHtml.AppendHtml(icon);
        }

        return tagBuilder;
    }

    private static TagBuilder GenerateStartButton()
    {
        var icon = new TagBuilder("svg");
        icon.MergeCssClass("govuk-button__start-icon");
        icon.MergeAttributes(new Dictionary<string, string?>()
        {
            { "xmlns", "http://www.w3.org/2000/svg" },
            { "width", "17.5" },
            { "height", "19" },
            { "viewBox", "0 0 33 40" },
            { "aria-hidden", "true" },
            { "focusable", "false" }
        });

        var path = new TagBuilder("path");
        path.MergeAttributes(new Dictionary<string, string?>()
        {
            { "fill", "currentColor" },
            { "d", "M0 0h13l20 20-20 20H0l20-20z" }
        });

        icon.InnerHtml.AppendHtml(path);

        return icon;
    }
}
