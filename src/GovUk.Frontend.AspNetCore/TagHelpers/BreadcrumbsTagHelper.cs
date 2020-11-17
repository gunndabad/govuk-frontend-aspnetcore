using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-breadcrumbs", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("govuk-breadcrumbs-item")]
    public class BreadcrumbsTagHelper : TagHelper
    {
        private const string CollapseOnMobileAttributeName = "collapse-on-mobile";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public BreadcrumbsTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        [HtmlAttributeName(CollapseOnMobileAttributeName)]
        public bool? CollapseOnMobile { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var bcContext = new BreadcrumbsContext();

            using (context.SetScopedContextItem(typeof(BreadcrumbsContext), bcContext))
            {
                await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateBreadcrumbs(
                CollapseOnMobile,
                output.Attributes.ToAttributesDictionary(),
                bcContext.Items);

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
        private const string LinkAttributesPrefix = "link-";

        public BreadcrumbsItemTagHelper(IGovUkHtmlGenerator htmlGenerator, IUrlHelperFactory urlHelperFactory)
            : base(htmlGenerator, urlHelperFactory)
        {
        }

        [HtmlAttributeName(DictionaryAttributePrefix = LinkAttributesPrefix)]
        public IDictionary<string, string> LinkAttributes { get; set; } = new Dictionary<string, string>();

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var bcContext = (BreadcrumbsContext)context.Items[typeof(BreadcrumbsContext)];

            var childContent = await output.GetChildContentAsync();

            var href = HasLinkAttributes ? ResolveHref() : null;

            bcContext.AddItem(new BreadcrumbsItem()
            {
                Attributes = output.Attributes.ToAttributesDictionary(),
                Href = href,
                LinkAttributes = LinkAttributes,
                Content = childContent.Snapshot()
            });

            output.SuppressOutput();
        }
    }

    internal class BreadcrumbsContext
    {
        private readonly List<BreadcrumbsItem> _items;

        public BreadcrumbsContext()
        {
            _items = new List<BreadcrumbsItem>();
        }

        public IReadOnlyCollection<BreadcrumbsItem> Items => _items;

        public void AddItem(BreadcrumbsItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _items.Add(item);
        }
    }
}
