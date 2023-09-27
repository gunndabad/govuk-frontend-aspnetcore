using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS accordion component.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(ComponentGenerator.AccordionElement)]
[RestrictChildren(AccordionItemTagHelper.TagName)]
public class AccordionTagHelper : TagHelper
{
    internal const string TagName = "govuk-accordion";

    private const string HeadingLevelAttributeName = "heading-level";
    private const string HideAllSectionsTextAttributeName = "hide-all-sections-text";
    private const string HideSectionTextAttributeName = "hide-section-text";
    private const string HideSectionAriaLabelTextAttributeName = "hide-section-aria-label-text";
    private const string IdAttributeName = "id";
    private const string RememberExpandedAttributeName = "remember-expanded";
    private const string ShowAllSectionsTextAttributeName = "show-all-sections-text";
    private const string ShowSectionTextAttributeName = "show-section-text";
    private const string ShowSectionAriaLabelTextAttributeName = "show-section-aria-label-text";

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
    /// The heading level.
    /// </summary>
    /// <remarks>
    /// Must be between <c>1</c> and <c>6</c> (inclusive). The default is <c>2</c>.
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
    /// The text content of the &quot;Hide all sections&quot; button at the top of the accordion when all sections
    /// are expanded.
    /// </summary>
    [HtmlAttributeName(HideAllSectionsTextAttributeName)]
    public string? HideAllSectionsText { get; set; }

    /// <summary>
    /// The text content of the &quot;Hide&quot; button within each section of the accordion, which is visible when the
    /// section is expanded.
    /// </summary>
    [HtmlAttributeName(HideSectionTextAttributeName)]
    public string? HideSectionText { get; set; }

    /// <summary>
    /// The text made available to assistive technologies, like screen-readers, as the final part of the toggle's
    /// accessible name when the section is expanded.
    /// </summary>
    /// <remarks>
    /// The default is <c>&quot;Hide this section&quot;</c>.
    /// </remarks>
    [HtmlAttributeName(HideSectionAriaLabelTextAttributeName)]
    public string? HideSectionAriaLabelText { get; set; }

    /// <summary>
    /// The <c>id</c> attribute for the accordion.
    /// </summary>
    /// <remarks>
    /// Must be unique across the domain of your service if <see cref="RememberExpanded"/> is <c>true</c>.
    /// Cannot be <c>null</c> or empty.
    /// </remarks>
    [HtmlAttributeName(IdAttributeName)]
    [DisallowNull]
    public string? Id
    {
        get => _id;
        set
        {
            _id = Guard.ArgumentNotNullOrEmpty(nameof(value), value);
        }
    }

    /// <summary>
    /// Whether the expanded/collapsed state of the accordion should be saved when a user leaves the page and restored when they return.
    /// </summary>
    /// <remarks>
    /// The default is <c>true</c>.
    /// </remarks>
    [HtmlAttributeName(RememberExpandedAttributeName)]
    public bool RememberExpanded { get; set; } = ComponentGenerator.AccordionDefaultRememberExpanded;

    /// <summary>
    /// The text content of the &quot;Show all sections&quot; button at the top of the accordion, which is visible when the
    /// section is collapsed.
    /// </summary>
    [HtmlAttributeName(ShowAllSectionsTextAttributeName)]
    public string? ShowAllSectionsText { get; set; }

    /// <summary>
    /// The text content of the &quot;Show&quot; button within each section of the accordion, which is visible when the
    /// section is collapsed.
    /// </summary>
    [HtmlAttributeName(ShowSectionTextAttributeName)]
    public string? ShowSectionText { get; set; }

    /// <summary>
    /// The text made available to assistive technologies, like screen-readers, as the final part of the toggle's
    /// accessible name when the section is collapsed.
    /// </summary>
    /// <remarks>
    /// The defaults is <c>&quot;Show this section&quot;</c>.
    /// </remarks>
    [HtmlAttributeName(ShowSectionAriaLabelTextAttributeName)]
    public string? ShowSectionAriaLabelText { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (Id == null)
        {
            throw ExceptionHelper.TheAttributeMustBeSpecified(IdAttributeName);
        }

        var accordionContext = new AccordionContext();

        using (context.SetScopedContextItem(accordionContext))
        {
            await output.GetChildContentAsync();
        }

        var tagBuilder = _htmlGenerator.GenerateAccordion(
            Id,
            HeadingLevel,
            output.Attributes.ToAttributeDictionary(),
            RememberExpanded,
            ShowAllSectionsText,
            ShowSectionText,
            ShowSectionAriaLabelText,
            HideAllSectionsText,
            HideSectionText,
            HideSectionAriaLabelText,
            items: accordionContext.Items);

        output.TagName = tagBuilder.TagName;
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Attributes.Clear();
        output.MergeAttributes(tagBuilder);
        output.Content.SetHtmlContent(tagBuilder.InnerHtml);
    }
}
