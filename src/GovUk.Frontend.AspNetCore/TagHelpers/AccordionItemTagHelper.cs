using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents an item in a GDS accordion component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = AccordionTagHelper.TagName)]
[OutputElementHint(ComponentGenerator.AccordionItemElement)]
[RestrictChildren(AccordionItemHeadingTagHelper.TagName, AccordionItemSummaryTagHelper.TagName, AccordionItemContentTagHelper.TagName)]
public class AccordionItemTagHelper : TagHelper
{
    internal const string TagName = "govuk-accordion-item";

    private const string ExpandedAttributeName = "expanded";

    /// <summary>
    /// Whether the section should be expanded upon initial load.
    /// </summary>
    /// <remarks>
    /// The default is <c>false</c>.
    /// </remarks>
    [HtmlAttributeName(ExpandedAttributeName)]
    public bool Expanded { get; set; } = ComponentGenerator.AccordionItemDefaultExpanded;

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var accordionContext = context.GetContextItem<AccordionContext>();

        var itemContext = new AccordionItemContext();

        using (context.SetScopedContextItem(itemContext))
        {
            await output.GetChildContentAsync();
        }

        itemContext.ThrowIfIncomplete();

        accordionContext.AddItem(new AccordionItem()
        {
            Expanded = Expanded,
            HeadingContent = itemContext.Heading!.Value.Content,
            HeadingAttributes = itemContext.Heading.Value.Attributes,
            SummaryContent = itemContext.Summary?.Content,
            SummaryAttributes = itemContext.Summary?.Attributes,
            Content = itemContext.Content?.Content,
            ContentAttributes = itemContext.Content?.Attributes,
            Attributes = output.Attributes.ToAttributeDictionary()
        });

        output.SuppressOutput();
    }
}
