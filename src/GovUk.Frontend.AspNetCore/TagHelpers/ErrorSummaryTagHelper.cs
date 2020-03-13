using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-error-summary")]
    [RestrictChildren("govuk-error-summary-title", "govuk-error-summary-description", "govuk-error-summary-item")]
    public class ErrorSummaryTagHelper : TagHelper
    {
        private const string AttributesPrefix = "error-summary-";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public ErrorSummaryTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.ThrowIfOutputHasAttributes();

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
                Attributes,
                errorSummaryContext.Items);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
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
        private const string AspForAttributeName = "asp-for";
        private const string AttributesPrefix = "error-summary-item-";
        private const string ForAttributeName = "for";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public ErrorSummaryItemTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        [HtmlAttributeName(AspForAttributeName)]
        public ModelExpression AspFor { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; }

        [HtmlAttributeName(ForAttributeName)]
        public string For { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (output.TagMode == TagMode.SelfClosing && AspFor == null)
            {
                throw new InvalidOperationException(
                    $"Content is required when the '{AspForAttributeName}' attribute is not specified.");
            }

            var errorSummaryContext = (ErrorSummaryContext)context.Items[ErrorSummaryContext.ContextName];

            var childContent = await output.GetChildContentAsync();

            IHtmlContent itemContent;

            if (output.TagMode == TagMode.StartTagAndEndTag)
            {
                itemContent = childContent.Snapshot();
            }
            else
            {
                var validationMessage = _htmlGenerator.GetValidationMessage(ViewContext, AspFor.ModelExplorer, AspFor.Name);

                if (validationMessage == null)
                {
                    return;
                }

                itemContent = new HtmlString(validationMessage);
            }

            // If there are link attributes or AspFor specified then wrap content in a link
            if (For != null || AspFor != null)
            {
                var resolvedHref = For != null ?
                    "#" + For :
                    "#" + TagBuilder.CreateSanitizedId(
                        _htmlGenerator.GetFullHtmlFieldName(ViewContext, AspFor.Name),
                        Constants.IdAttributeDotReplacement);

                var link = new TagBuilder("a");
                link.Attributes.Add("href", resolvedHref);
                link.InnerHtml.AppendHtml(itemContent);

                itemContent = link;
            }

            errorSummaryContext.AddItem(new ErrorSummaryItem()
            {
                Content = itemContent,
                Attributes = Attributes
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
