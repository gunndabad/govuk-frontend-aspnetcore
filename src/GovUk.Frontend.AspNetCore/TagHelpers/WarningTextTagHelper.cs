#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Generates a GDS warning text component.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.WarningTextElement)]
    public class WarningTextTagHelper : TagHelper
    {
        internal const string TagName = "govuk-warning-text";

        private const string IconFallbackTextAttributeName = "icon-fallback-text";

        private readonly IGovUkHtmlGenerator _htmlGenerator;
        private string? _iconFallbackText;

        /// <summary>
        /// Creates a new <see cref="WarningTextTagHelper"/>.
        /// </summary>
        public WarningTextTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal WarningTextTagHelper(IGovUkHtmlGenerator? htmlGenerator = null)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <summary>
        /// The fallback text for the icon.
        /// </summary>
        [HtmlAttributeName(IconFallbackTextAttributeName)]
        [DisallowNull]
        public string? IconFallbackText
        {
            get => _iconFallbackText;
            set
            {
                _iconFallbackText = Guard.ArgumentNotNullOrEmpty(nameof(value), value);
            }
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (IconFallbackText == null)
            {
                throw ExceptionHelper.TheAttributeMustBeSpecified(IconFallbackTextAttributeName);
            }

            var childContent = await output.GetChildContentAsync();

            var tagBuilder = _htmlGenerator.GenerateWarningText(
                IconFallbackText,
                childContent,
                output.Attributes.ToAttributesDictionary());

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
