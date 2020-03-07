using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-error-summary")]
    [RestrictChildren("govuk-error-summary-title", "govuk-error-summary-description", "govuk-error-summary-item")]
    public class ErrorSummaryTagHelper : TagHelper
    {
        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public ErrorSummaryTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var errorSummaryContext = new ErrorSummaryContext();

            using (context.SetScopedContextItem(ErrorSummaryContext.ContextName, errorSummaryContext))
            {
                await output.GetChildContentAsync();
            }

            if (errorSummaryContext.Title == null)
            {
                throw new InvalidOperationException("Missing <govuk-error-summary-title> element.");
            }

            var tagBuilder = _htmlGenerator.GenerateErrorSummary(
                errorSummaryContext.Title,
                errorSummaryContext.Description,
                errorSummaryContext.Items);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }

    [HtmlTargetElement("govuk-error-summary-title", ParentTag = "govuk-error-summary")]
    public class ErrorSummaryTitleTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var errorSummaryContext = (ErrorSummaryContext)context.Items[ErrorSummaryContext.ContextName];

            var childContent = await output.GetChildContentAsync();

            if (!errorSummaryContext.TrySetTitle(childContent.Snapshot()))
            {
                throw new InvalidOperationException($"Cannot render <{output.TagName}> here.");
            }

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-error-summary-description", ParentTag = "govuk-error-summary")]
    public class ErrorSummaryDescriptionTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var errorSummaryContext = (ErrorSummaryContext)context.Items[ErrorSummaryContext.ContextName];

            var childContent = await output.GetChildContentAsync();

            if (!errorSummaryContext.TrySetDescription(childContent.Snapshot()))
            {
                throw new InvalidOperationException($"Cannot render <{output.TagName}> here.");
            }

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-error-summary-item", ParentTag = "govuk-error-summary")]
    public class ErrorSummaryItemTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var errorSummaryContext = (ErrorSummaryContext)context.Items[ErrorSummaryContext.ContextName];

            var childContent = await output.GetChildContentAsync();

            errorSummaryContext.AddItem(new ErrorSummaryItem()
            {
                Content = childContent.Snapshot()
            });

            output.SuppressOutput();
        }
    }

    internal class ErrorSummaryContext
    {
        public const string ContextName = nameof(ErrorSummaryContext);

        private readonly List<ErrorSummaryItem> _items;

        public ErrorSummaryContext()
        {
            _items = new List<ErrorSummaryItem>();
        }

        public IReadOnlyCollection<ErrorSummaryItem> Items => _items;

        public IHtmlContent Description { get; private set; }

        public IHtmlContent Title { get; private set; }

        public void AddItem(ErrorSummaryItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _items.Add(item);
        }

        public bool TrySetDescription(IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Description != null)
            {
                return false;
            }

            Description = content;
            return true;
        }

        public bool TrySetTitle(IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Title != null)
            {
                return false;
            }

            Title = content;
            return true;
        }
    }
}
