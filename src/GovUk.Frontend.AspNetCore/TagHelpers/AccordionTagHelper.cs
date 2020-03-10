using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-accordion")]
    [RestrictChildren("govuk-accordion-items")]
    public class AccordionTagHelper : TagHelper
    {
        private const string IdAttributeName = "id";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public AccordionTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Id == null)
            {
                throw new InvalidOperationException($"The '{IdAttributeName}' attribute must be specified.");
            }

            var accordionContext = new AccordionContext();

            using (context.SetScopedContextItem(AccordionContext.ContextName, accordionContext))
            {
                var childContent = await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateAccordion(Id, accordionContext.Items);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }

    [HtmlTargetElement("govuk-accordion-items", ParentTag = "govuk-accordion")]
    [RestrictChildren("govuk-accordion-item")]
    public class AccordionItemsTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await output.GetChildContentAsync();

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-accordion-item", ParentTag = "govuk-accordion-items")]
    public class AccordionItemTagHelper : TagHelper
    {
        private const string ExpandedAttributeName = "expanded";

        [HtmlAttributeName(ExpandedAttributeName)]
        public bool Expanded { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var accordionContext = (AccordionContext)context.Items[AccordionContext.ContextName];

            var itemContext = new AccordionItemContext();

            TagHelperContent childContent;
            using (context.SetScopedContextItem(AccordionItemContext.ContextName, itemContext))
            {
                childContent = await output.GetChildContentAsync();
            }

            if (itemContext.Heading == null)
            {
                throw new InvalidOperationException("Missing <govuk-accordion-item-heading> element.");
            }

            accordionContext.AddItem(new AccordionItem()
            {
                Content = childContent.Snapshot(),
                Expanded = Expanded,
                HeadingContent = itemContext.Heading.Value.content,
                HeadingLevel = itemContext.Heading.Value.level,
                Summary = itemContext.Summary
            });

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-accordion-item-heading", ParentTag = "govuk-accordion-item")]
    public class AccordionItemHeadingTagHelper : TagHelper
    {
        private const string LevelAttributeName = "level";

        [HtmlAttributeName(LevelAttributeName)]
        public int? Level { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Level != null && (Level < 1 || Level > 6))
            {
                throw new InvalidOperationException($"The '{LevelAttributeName}' attribute must be between 1 and 6.");
            }

            var itemContext = (AccordionItemContext)context.Items[AccordionItemContext.ContextName];

            TagHelperContent childContent;
            using (context.SetScopedContextItem(AccordionItemContext.ContextName, itemContext))
            {
                childContent = await output.GetChildContentAsync();
            }

            if (!itemContext.TrySetHeading(Level, childContent.Snapshot()))
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
            var itemContext = (AccordionItemContext)context.Items[AccordionItemContext.ContextName];

            TagHelperContent childContent;
            using (context.SetScopedContextItem(AccordionItemContext.ContextName, itemContext))
            {
                childContent = await output.GetChildContentAsync();
            }

            if (!itemContext.TrySetSummary(childContent.Snapshot()))
            {
                throw new InvalidOperationException($"Cannot render <{output.TagName}> here.");
            }

            output.SuppressOutput();
        }
    }

    internal class AccordionContext
    {
        public const string ContextName = nameof(AccordionContext);

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
        public const string ContextName = nameof(AccordionItemContext);

        public (int? level, IHtmlContent content)? Heading { get; private set; }
        public IHtmlContent Summary { get; private set; }

        public bool TrySetHeading(int? level, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Heading != null)
            {
                return false;
            }

            Heading = (level, content);
            return true;
        }

        public bool TrySetSummary(IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Summary != null)
            {
                return false;
            }

            Summary = content;
            return true;
        }
    }
}
