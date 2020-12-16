#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const bool ButtonDefaultDisabled = false;
        internal const bool ButtonDefaultIsStartButton = false;
        internal const bool ButtonDefaultPreventDoubleClick = false;
        internal const string ButtonElement = "button";
        internal const string ButtonLinkElement = "a";

        public TagBuilder GenerateButton(
            bool isStartButton,
            bool disabled,
            bool preventDoubleClick,
            IHtmlContent content,
            IDictionary<string, string> attributes)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder(ButtonElement);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.AddCssClass("govuk-button");
            tagBuilder.Attributes.Add("data-module", "govuk-button");

            if (disabled)
            {
                tagBuilder.AddCssClass("govuk-button--disabled");
                tagBuilder.Attributes.Add("disabled", "disabled");
                tagBuilder.Attributes.Add("aria-disabled", "true");
            }

            if (preventDoubleClick)
            {
                tagBuilder.Attributes.Add("data-prevent-double-click", "true");
            }

            tagBuilder.InnerHtml.AppendHtml(content);

            if (isStartButton)
            {
                tagBuilder.AddCssClass("govuk-button--start");

                var icon = GenerateStartButton();
                tagBuilder.InnerHtml.AppendHtml(icon);
            }

            return tagBuilder;
        }

        public TagBuilder GenerateButtonLink(
            bool isStartButton,
            bool disabled,
            IHtmlContent content,
            IDictionary<string, string> attributes)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder(ButtonLinkElement);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.AddCssClass("govuk-button");
            tagBuilder.Attributes.Add("data-module", "govuk-button");
            tagBuilder.Attributes.Add("role", "button");
            tagBuilder.Attributes.Add("draggable", "false");

            if (disabled)
            {
                tagBuilder.AddCssClass("govuk-button--disabled");
            }

            tagBuilder.InnerHtml.AppendHtml(content);

            if (isStartButton)
            {
                tagBuilder.AddCssClass("govuk-button--start");

                var icon = GenerateStartButton();
                tagBuilder.InnerHtml.AppendHtml(icon);
            }

            return tagBuilder;
        }

        private static TagBuilder GenerateStartButton()
        {
            var icon = new TagBuilder("svg");
            icon.AddCssClass("govuk-button__start-icon");
            icon.MergeAttributes(new Dictionary<string, string>()
            {
                { "xmlns", "http://www.w3.org/2000/svg" },
                { "width", "17.5" },
                { "height", "19" },
                { "viewBox", "0 0 33 40" },
                { "aria-hidden", "true" },
                { "focusable", "false" }
            });

            var path = new TagBuilder("path");
            path.MergeAttributes(new Dictionary<string, string>()
            {
                { "fill", "currentColor" },
                { "d", "M0 0h13l20 20-20 20H0l20-20z" }
            });

            icon.InnerHtml.AppendHtml(path);

            return icon;
        }
    }
}
