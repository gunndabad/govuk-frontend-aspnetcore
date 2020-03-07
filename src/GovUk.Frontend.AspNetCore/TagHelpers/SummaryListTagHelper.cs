using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-summary-list")]
    [RestrictChildren("govuk-summary-list-row")]
    public class SummaryListTagHelper : TagHelper
    {
        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public SummaryListTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var summaryListContext = new SummaryListContext();

            using (context.SetScopedContextItem(SummaryListContext.ContextName, summaryListContext))
            {
                await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateSummaryList(summaryListContext.Rows);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }

    [HtmlTargetElement("govuk-summary-list-row", ParentTag = "govuk-summary-list")]
    [RestrictChildren("govuk-summary-list-row-key", "govuk-summary-list-row-value", "govuk-summary-list-row-actions")]
    public class SummaryListRowTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var summaryListContext = (SummaryListContext)context.Items[SummaryListContext.ContextName];

            var rowContext = new SummaryListRowContext();

            using (context.SetScopedContextItem(SummaryListRowContext.ContextName, rowContext))
            {
                await output.GetChildContentAsync();
            }

            summaryListContext.AddRow(new SummaryListRow()
            {
                Actions = rowContext.Actions,
                Key = rowContext.Key,
                Value = rowContext.Value
            });

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-summary-list-row-key", ParentTag = "govuk-summary-list-row")]
    public class SummaryListRowKeyTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var summaryListRowContext = (SummaryListRowContext)context.Items[SummaryListRowContext.ContextName];

            var content = await output.GetChildContentAsync();

            if (!summaryListRowContext.TrySetKey(content.Snapshot()))
            {
                throw new InvalidOperationException($"Cannot render <{context.TagName}> here.");
            }

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-summary-list-row-value", ParentTag = "govuk-summary-list-row")]
    public class SummaryListRowValueTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var summaryListRowContext = (SummaryListRowContext)context.Items[SummaryListRowContext.ContextName];

            var content = await output.GetChildContentAsync();

            if (!summaryListRowContext.TrySetValue(content.Snapshot()))
            {
                throw new InvalidOperationException($"Cannot render <{context.TagName}> here.");
            }

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-summary-list-row-actions", ParentTag = "govuk-summary-list-row")]
    public class SummaryListRowActionsTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await output.GetChildContentAsync();

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-summary-list-row-action", ParentTag = "govuk-summary-list-row-actions")]
    public class SummaryListRowActionTagHelper : LinkTagHelperBase
    {
        private const string VisuallyHiddenTextAttributeName = "visually-hidden-text";

        public SummaryListRowActionTagHelper(IGovUkHtmlGenerator htmlGenerator)
            : base(htmlGenerator)
        {
        }

        [HtmlAttributeName(VisuallyHiddenTextAttributeName)]
        public string VisuallyHiddenText { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var summaryListRowContext = (SummaryListRowContext)context.Items[SummaryListRowContext.ContextName];

            var href = ResolveHref();

            var content = await output.GetChildContentAsync();

            summaryListRowContext.AddAction(new SummaryListRowAction()
            {
                Content = content.Snapshot(),
                Href = href,
                VisuallyHiddenText = VisuallyHiddenText
            });

            output.SuppressOutput();
        }
    }

    internal class SummaryListContext
    {
        public const string ContextName = nameof(SummaryListContext);

        private readonly List<SummaryListRow> _rows;

        public SummaryListContext()
        {
            _rows = new List<SummaryListRow>();
        }

        public IReadOnlyCollection<SummaryListRow> Rows => _rows;

        public void AddRow(SummaryListRow row)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            _rows.Add(row);
        }
    }

    internal class SummaryListRowContext
    {
        public const string ContextName = nameof(SummaryListRowContext);

        private readonly List<SummaryListRowAction> _actions;

        public SummaryListRowContext()
        {
            _actions = new List<SummaryListRowAction>();
        }

        public IReadOnlyCollection<SummaryListRowAction> Actions => _actions;

        public IHtmlContent Key { get; private set; }

        public IHtmlContent Value { get; private set; }

        public void AddAction(SummaryListRowAction action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _actions.Add(action);
        }

        public bool TrySetKey(IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Key != null)
            {
                return false;
            }

            Key = content;
            return true;
        }

        public bool TrySetValue(IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Value != null)
            {
                return false;
            }

            Value = content;
            return true;
        }
    }
}
