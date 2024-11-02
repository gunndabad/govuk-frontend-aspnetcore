using System;
using System.Collections.Immutable;
using System.Diagnostics;
using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    /// <inheritdoc/>
    public virtual HtmlTag GenerateButton(ButtonOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        var classes = ($"govuk-button " + options.Classes).TrimEnd();
        var element =
            options.Element.NormalizeEmptyString()
            ?? (options.Href.NormalizeEmptyString() is not null ? "a" : "button");

        HtmlTag? iconHtml = null;
        if (options.IsStartButton == true)
        {
            iconHtml = new HtmlTag("svg")
                .AddClass("govuk-button__start-icon")
                .UnencodedAttr("xmlns", "http://www.w3.org/2000/svg")
                .UnencodedAttr("width", "17.5")
                .UnencodedAttr("height", "19")
                .UnencodedAttr("viewbox", "0 0 33 40")
                .UnencodedAttr("aria-hidden", "true")
                .UnencodedAttr("focusable", "false")
                .Append(
                    new HtmlTag("path")
                        .NoClosingTag()
                        .UnencodedAttr("fill", "currentColor")
                        .UnencodedAttr("d", "M0 0h13l20 20-20 20H0l20-20z")
                );

            classes += " govuk-button--start";
        }

        var commonAttributes = options
            .Attributes.ToImmutableDictionary()
            .Add("class", classes)
            .Add("data-module", "govuk-button")
            .AddIfNotNull("id", options.Id.NormalizeEmptyString());

        var buttonAttributes = ImmutableDictionary<string, string?>
            .Empty.AddIfNotNull("name", options.Name.NormalizeEmptyString())
            .AddIf(options.Disabled == true, "disabled", null)
            .AddIf(options.Disabled == true, "aria-disabled", "true")
            .AddIfNotNull("data-prevent-double-click", options.PreventDoubleClick?.ToString().ToLower());

        if (element == "a")
        {
            var button = new HtmlTag("a")
                .UnencodedAttr("href", options.Href.NormalizeEmptyString() ?? "#")
                .UnencodedAttr("role", "button")
                .UnencodedAttr("draggable", "false")
                .MergeEncodedAttributes(commonAttributes)
                .AppendHtml(GetEncodedTextOrHtml(options.Text, options.Html));

            if (iconHtml is not null)
            {
                button.Append(iconHtml);
            }

            return button;
        }
        else if (element == "button")
        {
            var button = new HtmlTag("button")
                .UnencodedAttr("type", options.Type.NormalizeEmptyString() ?? "submit")
                .MergeEncodedAttributes(buttonAttributes)
                .MergeEncodedAttributes(commonAttributes)
                .AppendHtml(GetEncodedTextOrHtml(options.Text, options.Html));

            if (options.Value.NormalizeEmptyString() is string value)
            {
                button.UnencodedAttr("value", value);
            }

            if (iconHtml is not null)
            {
                button.Append(iconHtml);
            }

            return button;
        }
        else
        {
            Debug.Assert(element == "input");
            Debug.Assert(options.Text.NormalizeEmptyString() is not null);

            return new HtmlTag("input")
                .UnencodedAttr("value", options.Text)
                .UnencodedAttr("type", options.Type.NormalizeEmptyString() ?? "submit")
                .MergeEncodedAttributes(buttonAttributes)
                .MergeEncodedAttributes(commonAttributes);
        }
    }
}
