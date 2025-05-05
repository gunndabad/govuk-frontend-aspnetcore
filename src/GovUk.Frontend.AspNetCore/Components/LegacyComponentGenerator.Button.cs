using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Components;

partial class LegacyComponentGenerator
{
    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateButton(ButtonOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        var classes = ($"govuk-button " + options.Classes).TrimEnd();
        var element = options.GetElement();

        HtmlTagBuilder? iconHtml = null;
        if (options.IsStartButton == true)
        {
            iconHtml = new HtmlTagBuilder("svg")
                .WithCssClass("govuk-button__start-icon")
                .WithAttribute("xmlns", "http://www.w3.org/2000/svg", encodeValue: false)
                .WithAttribute("width", "17.5", encodeValue: false)
                .WithAttribute("height", "19", encodeValue: false)
                .WithAttribute("viewbox", "0 0 33 40", encodeValue: false)
                .WithAttribute("aria-hidden", "true", encodeValue: false)
                .WithAttribute("focusable", "false", encodeValue: false)
                .WithAppendedHtml(new HtmlTagBuilder("path")
                    .WithAttribute("fill", "currentColor", encodeValue: false)
                    .WithAttribute("d", "M0 0h13l20 20-20 20H0l20-20z", encodeValue: false));

            classes += " govuk-button--start";
        }

        var commonAttributes = new EncodedAttributesDictionaryBuilder(options.Attributes)
            .WithCssClass(classes)
            .With("data-module", "govuk-button", encodeValue: false)
            .WithWhenNotNull(options.Id.NormalizeEmptyString(), "id");

        var buttonAttributes = new EncodedAttributesDictionaryBuilder()
            .WithWhenNotNull(options.Name.NormalizeEmptyString(), "name")
            .When(options.Disabled == true, b => b.WithBoolean("disabled").With("aria-disabled", "true", encodeValue: false))
            .WhenNotNull(options.PreventDoubleClick, (pdc, b) => b.With("data-prevent-double-click", pdc.ToString()!.ToLower(), encodeValue: false));

        if (element == "a")
        {
            var button = new HtmlTagBuilder("a")
                .WithAttribute("href", options.Href.NormalizeEmptyString() ?? new HtmlString("#"))
                .WithAttribute("role", "button", encodeValue: false)
                .WithAttribute("draggable", "false", encodeValue: false)
                .WithAttributes(commonAttributes)
                .WithAppendedHtml(GetEncodedTextOrHtml(options.Text, options.Html)!);

            if (iconHtml is not null)
            {
                button.WithAppendedHtml(iconHtml);
            }

            return button;
        }
        else if (element == "button")
        {
            var button = new HtmlTagBuilder("button")
                .WithAttribute("type", options.Type.NormalizeEmptyString() ?? new HtmlString("submit"))
                .WithAttributes(buttonAttributes)
                .WithAttributes(commonAttributes)
                .WithAppendedHtml(GetEncodedTextOrHtml(options.Text, options.Html)!);

            if (options.Value.NormalizeEmptyString() is IHtmlContent value)
            {
                button.WithAttribute("value", value);
            }

            if (iconHtml is not null)
            {
                button.WithAppendedHtml(iconHtml);
            }

            return button;
        }
        else
        {
            Debug.Assert(element == "input");
            Debug.Assert(options.Text.NormalizeEmptyString() is not null);

            return new HtmlTagBuilder("input")
                .WhenNotNull(options.Text, (value, b) => b.WithAttribute("value", value, encodeValue: true))
                .WithAttribute("type", options.Type.NormalizeEmptyString() ?? new HtmlString("submit"))
                .WithAttributes(buttonAttributes)
                .WithAttributes(commonAttributes);
        }
    }
}
