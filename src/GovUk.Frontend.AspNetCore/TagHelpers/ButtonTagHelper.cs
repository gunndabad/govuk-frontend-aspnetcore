using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-button")]
    public class ButtonTagHelper : LinkTagHelperBase
    {
        private const string FormActionAttributeName = "formaction";
        private const string IsDisabledAttributeName = "disabled";
        private const string IsStartButtonAttributeName = "is-start-button";
        private const string NameAttributeName = "name";
        private const string PreventDoubleClickAttributeName = "prevent-double-click";
        private const string TypeAttributeName = "type";
        private const string ValueAttributeName = "value";

        public ButtonTagHelper(IGovUkHtmlGenerator htmlGenerator, IUrlHelperFactory urlHelperFactory)
            : base(htmlGenerator, urlHelperFactory)
        {
        }

        [HtmlAttributeName(FormActionAttributeName)]
        public string FormAction { get; set; }

        [HtmlAttributeName(IsDisabledAttributeName)]
        public bool? IsDisabled { get; set; } = false;

        [HtmlAttributeName(IsStartButtonAttributeName)]
        public bool? IsStartButton { get; set; } = false;

        [HtmlAttributeName(NameAttributeName)]
        public string Name { get; set; }

        [HtmlAttributeName(PreventDoubleClickAttributeName)]
        public bool? PreventDoubleClick { get; set; } = false;

        [HtmlAttributeName(TypeAttributeName)]
        public string Type { get; set; }

        [HtmlAttributeName(ValueAttributeName)]
        public string Value { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var element = (Type == null && HasLinkAttributes)
                ? "a"
                : "button";
            output.TagName = element;

            if (element == "a" && Name != null)
            {
                throw new InvalidOperationException($"Cannot specify the '{NameAttributeName}' attribute for 'a' elements.");
            }

            if (element == "a" && PreventDoubleClick == true)
            {
                throw new InvalidOperationException($"Cannot specify the '{PreventDoubleClickAttributeName}' attribute for 'a' elements.");
            }

            if (element == "a" && Value != null)
            {
                throw new InvalidOperationException($"Cannot specify the '{ValueAttributeName}' attribute for '{element}' elements.");
            }

            if (element == "a" && FormAction != null)
            {
                throw new InvalidOperationException($"Cannot specify the '{FormActionAttributeName}' attribute for '{element}' elements.");
            }

            if (element == "button" && Href != null)
            {
                throw new InvalidOperationException($"Cannot specify the '{HrefAttributeName}' attribute for '{element}' elements.");
            }

            if (HasLinkAttributes && FormAction != null)
            {
                throw new InvalidOperationException(
                    $"Cannot determine the 'formaction' attribute for <button>. The following attributes are mutually exclusive:\n" +
                    $"{FormActionAttributeName}\n" +
                    $"{RouteAttributeName}\n" +
                    $"{ControllerAttributeName}, {ActionAttributeName}\n" +
                    $"{PageAttributeName}, {PageHandlerAttributeName}");
            }

            var childContent = await output.GetChildContentAsync();

            TagBuilder tagBuilder;

            if (element == "a")
            {
                var href = ResolveHref();

                tagBuilder = Generator.GenerateButtonLink(
                    href,
                    IsStartButton,
                    IsDisabled,
                    childContent,
                    output.Attributes.ToAttributesDictionary());
            }
            else
            {
                var resolvedFormAction = FormAction ??
                    (HasLinkAttributes ? ResolveHref() : null);

                tagBuilder = Generator.GenerateButton(
                    Name,
                    Type,
                    Value,
                    IsStartButton,
                    IsDisabled,
                    PreventDoubleClick,
                    resolvedFormAction,
                    childContent,
                    output.Attributes.ToAttributesDictionary());
            }

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}