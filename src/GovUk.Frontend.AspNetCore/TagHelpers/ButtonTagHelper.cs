using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    public enum ButtonTagHelperElementType { Anchor, Button }

    [HtmlTargetElement("govuk-button")]
    public class ButtonTagHelper : TagHelper
    {
        private const string DisabledAttributeName = "disabled";
        private const string ElementAttributeName = "element";
        private const string HrefAttributeName = "href";
        private const string IsStartButtonAttributeName = "is-start-button";
        private const string NameAttributeName = "name";
        private const string PreventDoubleClickAttributeName = "prevent-double-click";
        private const string TypeAttributeName = "type";
        private const string ValueAttributeName = "value";

        // Duplicate attributes for anchor tag pass through
        private const string ActionAttributeName = "asp-action";
        private const string ControllerAttributeName = "asp-controller";
        private const string AreaAttributeName = "asp-area";
        private const string PageAttributeName = "asp-page";
        private const string PageHandlerAttributeName = "asp-page-handler";
        private const string FragmentAttributeName = "asp-fragment";
        private const string HostAttributeName = "asp-host";
        private const string ProtocolAttributeName = "asp-protocol";
        private const string RouteAttributeName = "asp-route";
        private const string RouteValuesDictionaryName = "asp-all-route-data";
        private const string RouteValuesPrefix = "asp-route-";
        private IDictionary<string, string> _routeValues;

        private readonly IHtmlGenerator _htmlGenerator;

        public ButtonTagHelper(IHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator;
        }

        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        [HtmlAttributeName(AreaAttributeName)]
        public string Area { get; set; }

        [HtmlAttributeName(ControllerAttributeName)]
        public string Controller { get; set; }

        [HtmlAttributeName(DisabledAttributeName)]
        public bool Disabled { get; set; } = false;

        [HtmlAttributeName(ElementAttributeName)]
        public ButtonTagHelperElementType? Element { get; set; }

        [HtmlAttributeName(FragmentAttributeName)]
        public string Fragment { get; set; }

        [HtmlAttributeName(HostAttributeName)]
        public string Host { get; set; }

        [HtmlAttributeName(HrefAttributeName)]
        public string Href { get; set; }

        [HtmlAttributeName(IsStartButtonAttributeName)]
        public bool IsStartButton { get; set; } = false;

        [HtmlAttributeName(NameAttributeName)]
        public string Name { get; set; }

        [HtmlAttributeName(PageAttributeName)]
        public string Page { get; set; }

        [HtmlAttributeName(PageHandlerAttributeName)]
        public string PageHandler { get; set; }

        [HtmlAttributeName(PreventDoubleClickAttributeName)]
        public bool PreventDoubleClick { get; set; } = false;

        [HtmlAttributeName(ProtocolAttributeName)]
        public string Protocol { get; set; }

        [HtmlAttributeName(RouteAttributeName)]
        public string Route { get; set; }

        [HtmlAttributeName(RouteValuesDictionaryName, DictionaryAttributePrefix = RouteValuesPrefix)]
        public IDictionary<string, string> RouteValues
        {
            get
            {
                if (_routeValues == null)
                {
                    _routeValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }

                return _routeValues;
            }
            set
            {
                _routeValues = value;
            }
        }

        [HtmlAttributeName(TypeAttributeName)]
        public string Type { get; set; }  // TODO Make this an enum?

        [HtmlAttributeName(ValueAttributeName)]
        public string Value { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

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
                (_routeValues != null && _routeValues.Count > 0);

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
            output.TagMode = TagMode.SelfClosing;

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
                output.TagMode = TagMode.StartTagAndEndTag;

                if (Href != null)
                {
                    output.Attributes.Add("href", Href);
                }

                var anchorTagHelper = new AnchorTagHelper(_htmlGenerator)
                {
                    Action = Action,
                    Area = Area,
                    Controller = Controller,
                    Fragment = Fragment,
                    Host = Host,
                    Page = Page,
                    PageHandler = PageHandler,
                    Protocol = Protocol,
                    Route = Route,
                    RouteValues = RouteValues,
                    ViewContext = ViewContext
                };

                await anchorTagHelper.ProcessAsync(context, output);

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
                output.TagMode = TagMode.StartTagAndEndTag;

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