using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateButton(ButtonOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        var classes = ($"govuk-button " + options.Classes).TrimEnd();
        var element = options.Element.NormalizeEmptyString() ?? (options.Href.NormalizeEmptyString() is not null ? "a" : "button");

        HtmlTagBuilder? iconHtml = null;
        if (options.IsStartButton == true)
        {
            iconHtml = new HtmlTagBuilder("svg")
                .AddCssClass("govuk-button__start-icon")
                .AddAttribute("xmlns", "http://www.w3.org/2000/svg", encodeValue: false)
                .AddAttribute("width", "17.5", encodeValue: false)
                .AddAttribute("height", "19", encodeValue: false)
                .AddAttribute("viewbox", "0 0 33 40", encodeValue: false)
                .AddAttribute("aria-hidden", "true", encodeValue: false)
                .AddAttribute("focusable", "false", encodeValue: false)
                .AppendHtml(new HtmlTagBuilder("path")
                    .NoClosingTag()
                    .AddAttribute("fill", "currentColor")
                    .AddAttribute("d", "M0 0h13l20 20-20 20H0l20-20z"));

            classes += " govuk-button--start";
        }

        var commonAttributes = (options.Attributes?.Clone() ?? [])
            .Add("class", classes, encodeValue: false)
            .Add("data-module", "govuk-button", encodeValue: false)
            .AddIfNotNull("id", options.Id.NormalizeEmptyString());

        var buttonAttributes = new EncodedAttributesDictionary()
            .AddIfNotNull("name", options.Name.NormalizeEmptyString())
            .AddIf(options.Disabled == true, "disabled", null)
            .AddIf(options.Disabled == true, "aria-disabled", "true")
            .AddIfNotNull("data-prevent-double-click", options.PreventDoubleClick?.ToString().ToLower());

        if (element == "a")
        {
            var button = new HtmlTagBuilder("a")
                .AddAttribute("href", options.Href.NormalizeEmptyString() ?? "#", encodeValue: false)
                .AddAttribute("role", "button", encodeValue: false)
                .AddAttribute("draggable", "false", encodeValue: false)
                .AddAttributes(commonAttributes)
                .AppendHtml(GetEncodedTextOrHtml(options.Text, options.Html));

            if (iconHtml is not null)
            {
                button.AppendHtml(iconHtml);
            }

            return button;
        }
        else if (element == "button")
        {
            var button = new HtmlTagBuilder("button")
                .AddAttribute("type", options.Type.NormalizeEmptyString() ?? "submit", encodeValue: false)
                .AddAttributes(buttonAttributes)
                .AddAttributes(commonAttributes)
                .AppendHtml(GetEncodedTextOrHtml(options.Text, options.Html));

            if (options.Value.NormalizeEmptyString() is string value)
            {
                button.AddAttribute("value", value, encodeValue: false);
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

            return new HtmlTagBuilder("input")
                .AddAttribute("value", options.Value, encodeValue: false)
                .AddAttribute("type", options.Type.NormalizeEmptyString() ?? "submit", encodeValue: false)
                .AddAttributes(buttonAttributes)
                .AddAttributes(commonAttributes);
        }
    }
}
