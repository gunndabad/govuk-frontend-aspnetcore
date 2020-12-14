#nullable enable
using System;
#if !NETSTANDARD2_0
using System.Diagnostics.CodeAnalysis;
#endif
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.AccordionElement)]
    [RestrictChildren(AccordionItemTagHelper.TagName)]
    public class AccordionTagHelper : TagHelper
    {
        internal const string TagName = "govuk-accordion";

        private const string HeadingLevelAttributeName = "heading-level";
        private const string IdAttributeName = "id";

        private readonly IGovUkHtmlGenerator _htmlGenerator;
        private string? _id;
        private int _headingLevel = ComponentGenerator.AccordionDefaultHeadingLevel;

        /// <summary>
        /// Creates a new <see cref="AccordionTagHelper"/>.
        /// </summary>
        public AccordionTagHelper()
            : this(null)
        {
        }

        internal AccordionTagHelper(IGovUkHtmlGenerator? htmlGenerator = null)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <summary>
        /// The heading level for the accordion.
        /// </summary>
        /// <remarks>
        /// Must be between 1 and 6 (inclusive). The default is 2.
        /// </remarks>
        [HtmlAttributeName(HeadingLevelAttributeName)]
        public int HeadingLevel
        {
            get => _headingLevel;
            set
            {
                if (value < ComponentGenerator.AccordionMinHeadingLevel ||
                    value > ComponentGenerator.AccordionMaxHeadingLevel)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        $"{nameof(HeadingLevel)} must be between {ComponentGenerator.AccordionMinHeadingLevel} and {ComponentGenerator.AccordionMaxHeadingLevel}.");
                }

                _headingLevel = value;
            }
        }

        /// <summary>
        /// The id attribute for the accordion.
        /// </summary>
        /// <remarks>
        /// Must be unique across the domain of your service.
        /// Cannot be <see langword="null"/>.
        /// </remarks>
        [HtmlAttributeName(IdAttributeName)]
#if !NETSTANDARD2_0
        [DisallowNull]
#endif
        public string? Id
        {
            get => _id;
            set
            {
                _id = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Id == null
#if TEST_SWITCHES
                && !Config.RunningConformanceTests
#endif
                )
            {
                throw ExceptionHelper.TheAttributeMustBeSpecified(IdAttributeName);
            }

            var accordionContext = new AccordionContext();

            using (context.SetScopedContextItem(accordionContext))
            {
                await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateAccordion(
                Id
#if TEST_SWITCHES
                ?? string.Empty
#endif
                ,
                HeadingLevel,
                output.Attributes.ToAttributesDictionary(),
                accordionContext.Items);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
