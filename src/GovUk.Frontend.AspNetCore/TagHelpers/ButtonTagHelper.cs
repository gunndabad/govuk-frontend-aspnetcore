#nullable enable
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Generates a GDS button component that renders a &lt;button&gt; element.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.ButtonElement)]
    public class ButtonTagHelper : TagHelper
    {
        internal const string TagName = "govuk-button";

        private const string DisabledAttributeName = "disabled";
        private const string IsStartButtonAttributeName = "is-start-button";
        private const string PreventDoubleClickAttributeName = "prevent-double-click";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a new <see cref="ButtonTagHelper"/>.
        /// </summary>
        public ButtonTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal ButtonTagHelper(IGovUkHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <summary>
        /// Whether the button should be disabled.
        /// </summary>
        /// <remarks>
        /// The default is <c>false</c>.
        /// </remarks>
        [HtmlAttributeName(DisabledAttributeName)]
        public bool Disabled { get; set; } = ComponentGenerator.ButtonDefaultDisabled;

        /// <summary>
        /// Whether this button is the main call to action on your service's start page.
        /// </summary>
        /// <remarks>
        /// The default is <c>false</c>.
        /// </remarks>
        [HtmlAttributeName(IsStartButtonAttributeName)]
        public bool IsStartButton { get; set; } = ComponentGenerator.ButtonDefaultIsStartButton;

        /// <summary>
        /// Whether to prevent accidental double clicks on submit buttons from submitting forms multiple times.
        /// </summary>
        /// <remarks>
        /// The default is <c>false</c>.
        /// </remarks>
        [HtmlAttributeName(PreventDoubleClickAttributeName)]
        public bool PreventDoubleClick { get; set; } = ComponentGenerator.ButtonDefaultPreventDoubleClick;

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            var tagBuilder = _htmlGenerator.GenerateButton(
                IsStartButton,
                Disabled,
                PreventDoubleClick,
                childContent,
                output.Attributes.ToAttributeDictionary());

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
