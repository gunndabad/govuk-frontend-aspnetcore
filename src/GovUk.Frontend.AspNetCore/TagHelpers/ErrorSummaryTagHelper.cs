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
        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public ErrorSummaryTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var errorSummaryContext = new ErrorSummaryContext();

            using (context.SetScopedContextItem(typeof(ErrorSummaryContext), errorSummaryContext))
            {
                await output.GetChildContentAsync();
            }

            if (errorSummaryContext.Title == null &&
                errorSummaryContext.Description == null &&
                errorSummaryContext.Items.Count == 0)
            {
                output.SuppressOutput();
                return;
            }

            var tagBuilder = _htmlGenerator.GenerateErrorSummary(
                errorSummaryContext.Title?.content ?? new HtmlString(ComponentDefaults.ErrorSummary.Title),
                errorSummaryContext.Title?.attributes,
                errorSummaryContext.Description?.content,
                errorSummaryContext.Description?.attributes,
                output.Attributes.ToAttributeDictionary(),
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
            var errorSummaryContext = (ErrorSummaryContext)context.Items[typeof(ErrorSummaryContext)];

            var childContent = await output.GetChildContentAsync();

            if (!errorSummaryContext.TrySetTitle(output.Attributes.ToAttributeDictionary(), childContent.Snapshot()))
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
            var errorSummaryContext = (ErrorSummaryContext)context.Items[typeof(ErrorSummaryContext)];

            var childContent = await output.GetChildContentAsync();

            if (!errorSummaryContext.TrySetDescription(output.Attributes.ToAttributeDictionary(), childContent.Snapshot()))
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
        private const string ForAttributeName = "for";

        private readonly IGovUkHtmlGenerator _htmlGenerator;
        private readonly IModelHelper _modelHelper;

        public ErrorSummaryItemTagHelper(
            IGovUkHtmlGenerator htmlGenerator,
            IModelHelper modelHelper)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
            _modelHelper = modelHelper ?? throw new ArgumentNullException(nameof(modelHelper));
        }

        [HtmlAttributeName(AspForAttributeName)]
        public ModelExpression AspFor { get; set; }

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

            var errorSummaryContext = (ErrorSummaryContext)context.Items[typeof(ErrorSummaryContext)];

            var childContent = await output.GetChildContentAsync();

            IHtmlContent itemContent;

            if (output.TagMode == TagMode.StartTagAndEndTag)
            {
                itemContent = childContent.Snapshot();
            }
            else
            {
                var validationMessage = _modelHelper.GetValidationMessage(
                    ViewContext,
                    AspFor.ModelExplorer,
                    AspFor.Name);

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
                        _modelHelper.GetFullHtmlFieldName(ViewContext, AspFor.Name),
                        Constants.IdAttributeDotReplacement);

                var link = new TagBuilder("a");
                link.Attributes.Add("href", resolvedHref);
                link.InnerHtml.AppendHtml(itemContent);

                itemContent = link;
            }

            errorSummaryContext.AddItem(new ErrorSummaryItem()
            {
                Content = itemContent,
                Attributes = output.Attributes.ToAttributeDictionary()
            });

            output.SuppressOutput();
        }
    }

    internal class ErrorSummaryContext
    {
        private readonly List<ErrorSummaryItem> _items;

        public ErrorSummaryContext()
        {
            _items = new List<ErrorSummaryItem>();
        }

        public IReadOnlyCollection<ErrorSummaryItem> Items => _items;

        public (AttributeDictionary attributes, IHtmlContent content)? Description { get; private set; }

        public (AttributeDictionary attributes, IHtmlContent content)? Title { get; private set; }

        public void AddItem(ErrorSummaryItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _items.Add(item);
        }

        public bool TrySetDescription(AttributeDictionary attributes, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Description != null)
            {
                return false;
            }

            Description = (attributes, content);
            return true;
        }

        public bool TrySetTitle(AttributeDictionary attributes, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Title != null)
            {
                return false;
            }

            Title = (attributes, content);
            return true;
        }
    }
}
