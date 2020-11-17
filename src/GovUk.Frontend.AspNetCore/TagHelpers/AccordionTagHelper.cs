using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-accordion")]
    [RestrictChildren("govuk-accordion-item")]
    public class AccordionTagHelper : TagHelper
    {
        private const string HeadingLevelAttributeName = "heading-level";
        private const string IdAttributeName = "id";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public AccordionTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        [HtmlAttributeName(HeadingLevelAttributeName)]
        public int? HeadingLevel { get; set; }

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Id == null)
            {
                throw new InvalidOperationException($"The '{IdAttributeName}' attribute must be specified.");
            }

            if (HeadingLevel != null && (HeadingLevel < 1 || HeadingLevel > 6))
            {
                throw new InvalidOperationException(
                    $"The '{HeadingLevelAttributeName}' attribute must be between 1 and 6.");
            }

            var accordionContext = new AccordionContext();

            using (context.SetScopedContextItem(typeof(AccordionContext), accordionContext))
            {
                await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateAccordion(
                Id,
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

    [HtmlTargetElement("govuk-accordion-item", ParentTag = "govuk-accordion")]
    public class AccordionItemTagHelper : TagHelper
    {
        private const string IsExpandedAttributeName = "expanded";

        [HtmlAttributeName(IsExpandedAttributeName)]
        public bool IsExpanded { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var accordionContext = (AccordionContext)context.Items[typeof(AccordionContext)];

            var itemContext = new AccordionItemContext();

            TagHelperContent childContent;
            using (context.SetScopedContextItem(typeof(AccordionItemContext), itemContext))
            {
                childContent = await output.GetChildContentAsync();
            }

            if (itemContext.Heading == null)
            {
                throw new InvalidOperationException("Missing <govuk-accordion-item-heading> element.");
            }

            accordionContext.AddItem(new AccordionItem()
            {
                Attributes = output.Attributes.ToAttributesDictionary(),
                Content = childContent.Snapshot(),
                IsExpanded = IsExpanded,
                HeadingContent = itemContext.Heading.Value.content,
                HeadingAttributes = itemContext.Heading.Value.attributes,
                SummaryContent = itemContext.Summary?.content,
                SummaryAttributes = itemContext.Summary?.attributes
            });

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-accordion-item-heading", ParentTag = "govuk-accordion-item")]
    public class AccordionItemHeadingTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var itemContext = (AccordionItemContext)context.Items[typeof(AccordionItemContext)];

            TagHelperContent childContent;
            using (context.SetScopedContextItem(typeof(AccordionItemContext), itemContext))
            {
                childContent = await output.GetChildContentAsync();
            }

            if (!itemContext.TrySetHeading(output.Attributes.ToAttributesDictionary(), childContent.Snapshot()))
            {
                throw new InvalidOperationException($"Cannot render <{output.TagName}> here.");
            }

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-accordion-item-summary", ParentTag = "govuk-accordion-item")]
    public class AccordionItemSummaryTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var itemContext = (AccordionItemContext)context.Items[typeof(AccordionItemContext)];

            TagHelperContent childContent;
            using (context.SetScopedContextItem(typeof(AccordionItemContext), itemContext))
            {
                childContent = await output.GetChildContentAsync();
            }

            if (!itemContext.TrySetSummary(output.Attributes.ToAttributesDictionary(), childContent.Snapshot()))
            {
                throw new InvalidOperationException($"Cannot render <{output.TagName}> here.");
            }

            output.SuppressOutput();
        }
    }

    internal class AccordionContext
    {
        private readonly List<AccordionItem> _items;

        public AccordionContext()
        {
            _items = new List<AccordionItem>();
        }

        public IReadOnlyCollection<AccordionItem> Items => _items;

        internal void AddItem(AccordionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _items.Add(item);
        }
    }

    internal class AccordionItemContext
    {
        public (IDictionary<string, string> attributes, IHtmlContent content)? Heading { get; private set; }
        public (IDictionary<string, string> attributes, IHtmlContent content)? Summary { get; private set; }

        public bool TrySetHeading(IDictionary<string, string> attributes, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Heading != null)
            {
                return false;
            }

            Heading = (attributes, content);
            return true;
        }

        public bool TrySetSummary(IDictionary<string, string> attributes, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Summary != null)
            {
                return false;
            }

            Summary = (attributes, content);
            return true;
        }
    }
}
