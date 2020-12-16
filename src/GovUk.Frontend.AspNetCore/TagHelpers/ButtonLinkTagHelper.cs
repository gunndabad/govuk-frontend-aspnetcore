#nullable enable
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Generates a GDS button component that renders an &lt;a&gt; element.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.ButtonLinkElement)]
    public class ButtonLinkTagHelper : TagHelper
    {
        internal const string TagName = "govuk-button-link";

        private const string DisabledAttributeName = "disabled";
        private const string IsStartButtonAttributeName = "is-start-button";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a <see cref="ButtonLinkTagHelper"/>.
        /// </summary>
        public ButtonLinkTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal ButtonLinkTagHelper(IGovUkHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <summary>
        /// Whether the button should be disabled.
        /// </summary>
        /// <remarks>
        /// Defaults to <see langword="false"/>.
        /// </remarks>
        [HtmlAttributeName(DisabledAttributeName)]
        public bool Disabled { get; set; } = ComponentGenerator.ButtonDefaultDisabled;

        /// <summary>
        /// Whether this button is the main call to action on your service's start page.
        /// </summary>
        /// <remarks>
        /// Defaults to <see langword="false"/>.
        /// </remarks>
        [HtmlAttributeName(IsStartButtonAttributeName)]
        public bool IsStartButton { get; set; } = ComponentGenerator.ButtonDefaultIsStartButton;

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            var tagBuilder = _htmlGenerator.GenerateButtonLink(
                IsStartButton,
                Disabled,
                childContent.Snapshot(),
                output.Attributes.ToAttributesDictionary());

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
