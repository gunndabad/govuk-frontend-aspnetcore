using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    public enum ButtonTagHelperElementType { Anchor, Button }

    [HtmlTargetElement("govuk-button")]
    public class ButtonTagHelper : LinkTagHelperBase
    {
        private const string DisabledAttributeName = "disabled";
        private const string ElementAttributeName = "element";
        private const string IsStartButtonAttributeName = "is-start-button";
        private const string NameAttributeName = "name";
        private const string PreventDoubleClickAttributeName = "prevent-double-click";
        private const string TypeAttributeName = "type";
        private const string ValueAttributeName = "value";

        public ButtonTagHelper(IGovUkHtmlGenerator htmlGenerator)
            : base(htmlGenerator)
        {
        }

        [HtmlAttributeName(DisabledAttributeName)]
        public bool Disabled { get; set; } = false;

        [HtmlAttributeName(ElementAttributeName)]
        public ButtonTagHelperElementType? Element { get; set; }

        [HtmlAttributeName(IsStartButtonAttributeName)]
        public bool IsStartButton { get; set; } = false;

        [HtmlAttributeName(NameAttributeName)]
        public string Name { get; set; }

        [HtmlAttributeName(PreventDoubleClickAttributeName)]
        public bool PreventDoubleClick { get; set; } = false;

        [HtmlAttributeName(TypeAttributeName)]
        public string Type { get; set; }  // TODO Make this an enum?

        [HtmlAttributeName(ValueAttributeName)]
        public string Value { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var hasAnchorAttributes = Href != null ||
                Action != null ||
                Controller != null ||
                Area != null ||
                Page != null ||
                PageHandler != null ||
                Route != null ||
                Protocol != null ||
                Host != null ||
                Fragment != null ||
                RouteValues.Count > 0;

            string element = GetTagNameFromElementType(
                Element ??
                (hasAnchorAttributes ? ButtonTagHelperElementType.Anchor : ButtonTagHelperElementType.Button));
            output.TagName = element;

            if (element == "a" && Name != null)
            {
                throw new InvalidOperationException($"Cannot specify the '{NameAttributeName}' attribute for 'a' elements.");
            }

            if (element == "a" && PreventDoubleClick == true)
            {
                throw new InvalidOperationException($"Cannot specify the '{PreventDoubleClickAttributeName}' attribute for 'a' elements.");
            }

            if (element == "a" && Type != null)
            {
                throw new InvalidOperationException($"Cannot specify the '{TypeAttributeName}' attribute for 'a' elements.");
            }

            if (element == "a" && Value != null)
            {
                throw new InvalidOperationException($"Cannot specify the '{ValueAttributeName}' attribute for '{element}' elements.");
            }

            output.TagName = element;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();

            output.AddClass("govuk-button", HtmlEncoder.Default);

            if (Disabled)
            {
                output.AddClass("govuk-button--disabled", HtmlEncoder.Default);
            }

            TagBuilder icon = null;
            if (IsStartButton)
            {
                icon = new TagBuilder("svg");
                icon.AddCssClass("govuk-button__start-icon");
                icon.MergeAttributes(new Dictionary<string, string>()
                {
                    { "xmlns", "http://www.w3.org/2000/svg" },
                    { "width", "17.5" },
                    { "height", "19" },
                    { "viewBox", "0 0 33 40" },
                    { "role", "presentation" },
                    { "focusable", "false" }
                });

                var path = new TagBuilder("path");
                path.MergeAttributes(new Dictionary<string, string>()
                {
                    { "fill", "currentColor" },
                    { "d", "M0 0h13l20 20-20 20H0l20-20z" }
                });

                icon.InnerHtml.AppendHtml(path);

                output.AddClass("govuk-button--start", HtmlEncoder.Default);
            }

            output.Attributes.SetAttribute("data-module", "govuk-button");

            var childContent = await output.GetChildContentAsync();

            if (element == "a")
            {
                var anchorTagBuilder = CreateAnchorTagBuilder();

                output.MergeAttributes(anchorTagBuilder);
                output.Attributes.SetAttribute("role", "button");
                output.Attributes.SetAttribute("draggable", "false");

                output.Content.AppendHtml(childContent);

                if (icon != null)
                {
                    output.Content.AppendHtml(icon);
                }
            }
            else if (element == "button")
            {
                if (Name != null)
                {
                    output.Attributes.Add("name", Name);
                }

                if (Disabled)
                {
                    output.Attributes.Add("disabled", "disabled");
                    output.Attributes.Add("aria-disabled", "true");
                }

                if (PreventDoubleClick)
                {
                    output.Attributes.Add("data-prevent-double-click", "true");
                }

                if (Value != null)
                {
                    output.Attributes.SetAttribute("value", Value);
                }

                if (Type != null)
                {
                    output.Attributes.SetAttribute("type", Type);
                }

                output.Content.AppendHtml(childContent);

                if (icon != null)
                {
                    output.Content.AppendHtml(icon);
                }
            }
        }

        private string GetTagNameFromElementType(ButtonTagHelperElementType elementType) =>
            elementType switch
            {
                ButtonTagHelperElementType.Anchor => "a",
                ButtonTagHelperElementType.Button => "button",
                _ => throw new ArgumentException("Invalid elementType", nameof(elementType))
            };
    }
}