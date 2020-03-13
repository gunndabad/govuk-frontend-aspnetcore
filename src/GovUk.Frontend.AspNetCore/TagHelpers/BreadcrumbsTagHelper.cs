using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-breadcrumbs", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("govuk-breadcrumbs-item")]
    public class BreadcrumbsTagHelper : TagHelper
    {
        private const string AttributesPrefix = "breadcrumbs-";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public BreadcrumbsTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.ThrowIfOutputHasAttributes();

            var bcContext = new BreadcrumbsContext();

            using (context.SetScopedContextItem(BreadcrumbsContext.ContextName, bcContext))
            {
                await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateBreadcrumbs(Attributes, bcContext.Items);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }

    [HtmlTargetElement("govuk-breadcrumbs-item", TagStructure = TagStructure.NormalOrSelfClosing, ParentTag = "govuk-breadcrumbs")]
    public class BreadcrumbsItemTagHelper : LinkTagHelperBase
    {
        private const string IsCurrentPageAttributeName = "is-current-page";

        public BreadcrumbsItemTagHelper(IGovUkHtmlGenerator htmlGenerator, IUrlHelperFactory urlHelperFactory)
            : base(htmlGenerator, urlHelperFactory)
        {
        }

        [HtmlAttributeName(IsCurrentPageAttributeName)]
        public bool IsCurrentPage { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.ThrowIfOutputHasAttributes();

            var bcContext = (BreadcrumbsContext)context.Items[BreadcrumbsContext.ContextName];

            if (bcContext.HasCurrentPageItem)
            {
                if (IsCurrentPage)
                {
                    throw new InvalidOperationException($"Only one item with the '{IsCurrentPageAttributeName}' attribute set to 'true' can be specified.");
                }
                else
                {
                    throw new InvalidOperationException("Items cannot be added after the item representing the current page has been added.");
                }
            }

            if (IsCurrentPage && HasLinkAttributes)
            {
                throw new InvalidOperationException("The item representing the current page cannot be a link.");
            }

            var childContent = await output.GetChildContentAsync();

            var href = HasLinkAttributes ? ResolveHref() : null;

            bcContext.AddItem(new BreadcrumbsItem()
            {
                Href = href,
                Content = childContent.Snapshot(),
                IsCurrentPage = IsCurrentPage
            });

            output.SuppressOutput();
        }
    }

    internal class BreadcrumbsContext
    {
        public const string ContextName = nameof(BreadcrumbsContext);

        private readonly List<BreadcrumbsItem> _items;

        public BreadcrumbsContext()
        {
            _items = new List<BreadcrumbsItem>();
        }

        public bool HasCurrentPageItem => Items.Any(i => i.IsCurrentPage);

        public IReadOnlyCollection<BreadcrumbsItem> Items => _items;

        public void AddItem(BreadcrumbsItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (item.IsCurrentPage && HasCurrentPageItem)
            {
                throw new InvalidOperationException("An item representing the current page has already been added.");
            }

            _items.Add(item);
        }
    }
}
