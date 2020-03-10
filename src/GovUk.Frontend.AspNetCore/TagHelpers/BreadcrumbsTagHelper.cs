using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-breadcrumbs", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("govuk-breadcrumbs-item")]
    public class BreadcrumbsTagHelper : TagHelper
    {
        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public BreadcrumbsTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var bcContext = new BreadcrumbsContext();

            using (context.SetScopedContextItem(BreadcrumbsContext.ContextName, bcContext))
            {
                await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateBreadcrumbs(bcContext.Items, bcContext.CurrentPageItem);

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

        public BreadcrumbsItemTagHelper(IGovUkHtmlGenerator htmlGenerator)
            : base(htmlGenerator)
        {
        }

        [HtmlAttributeName(IsCurrentPageAttributeName)]
        public bool IsCurrentPage { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var isLink = Href != null ||
                Route != null ||
                Controller != null ||
                Action != null ||
                Page != null ||
                PageHandler != null;

            var childContent = await output.GetChildContentAsync();

            IHtmlContent content;
            if (isLink)
            {
                var link = CreateAnchorTagBuilder();
                link.AddCssClass("govuk-breadcrumbs__link");

                link.InnerHtml.AppendHtml(childContent);

                content = link;
            }
            else
            {
                content = childContent;
            }

            var bcContext = (BreadcrumbsContext)context.Items[BreadcrumbsContext.ContextName];

            if (IsCurrentPage)
            {
                bcContext.AddCurrentPageItem(content);
            }
            else
            {
                bcContext.AddItem(content);
            }

            output.SuppressOutput();
        }
    }

    internal class BreadcrumbsContext
    {
        public const string ContextName = nameof(BreadcrumbsContext);

        private readonly List<IHtmlContent> _items;

        public BreadcrumbsContext()
        {
            _items = new List<IHtmlContent>();
        }

        public IHtmlContent CurrentPageItem { get; private set; }

        public IReadOnlyCollection<IHtmlContent> Items => _items;

        public void AddItem(IHtmlContent item)
        {
            ThrowIfHaveCurrentPageItem();

            _items.Add(item);
        }

        public void AddCurrentPageItem(IHtmlContent item)
        {
            ThrowIfHaveCurrentPageItem();

            CurrentPageItem = item;
            _items.Add(item);
        }

        private void ThrowIfHaveCurrentPageItem()
        {
            if (CurrentPageItem != null)
            {
                throw new InvalidOperationException("An item representing the current page has already been added.");
            }
        }
    }
}
