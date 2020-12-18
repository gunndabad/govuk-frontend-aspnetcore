#nullable enable
using System;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents an item in a GDS accordion component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = AccordionTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.AccordionItemElement)]
    public class AccordionItemTagHelper : TagHelper
    {
        internal const string TagName = "govuk-accordion-item";

        private const string ExpandedAttributeName = "expanded";

        /// <summary>
        /// Whether the section should be expanded upon initial load or not.
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

            TagHelperContent childContent;
            using (context.SetScopedContextItem(itemContext))
            {
                childContent = await output.GetChildContentAsync();
            }

            if (itemContext.Heading == null)
            {
                throw new InvalidOperationException($"Missing <{AccordionItemHeadingTagHelper.TagName}> element.");
            }

            accordionContext.AddItem(new AccordionItem()
            {
                Expanded = Expanded,
                HeadingContent = itemContext.Heading.Value.content,
                HeadingAttributes = itemContext.Heading.Value.attributes,
                SummaryContent = itemContext.Summary?.content,
                SummaryAttributes = itemContext.Summary?.attributes,
                Content = childContent.Snapshot(),
                Attributes = output.Attributes.ToAttributesDictionary()
            });

            output.SuppressOutput();
        }
    }
}
