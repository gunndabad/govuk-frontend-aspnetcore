#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents an item in a GDS breadcrumbs component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = BreadcrumbsTagHelper.TagName)]
    //[OutputElementHint(ComponentGenerator.BreadcrumbsItemElement)]  // Omitted since it produces intellisense warnings
    public class BreadcrumbsItemTagHelper : LinkTagHelperBase
    {
        internal const string TagName = "govuk-breadcrumbs-item";

        private const string LinkAttributesPrefix = "link-";

        /// <summary>
        /// Creates a new <see cref="BreadcrumbsItemTagHelper"/>.
        /// </summary>
        /// <param name="urlHelperFactory">The <see cref="IUrlHelperFactory"/>.</param>
        public BreadcrumbsItemTagHelper(IUrlHelperFactory urlHelperFactory)
            : this(null, urlHelperFactory)
        {
        }

        internal BreadcrumbsItemTagHelper(IGovUkHtmlGenerator? htmlGenerator, IUrlHelperFactory urlHelperFactory)
            : base(htmlGenerator ?? new ComponentGenerator(), urlHelperFactory)
        {
        }

        /// <summary>
        /// Additional attributes for the generated <c>a</c> element.
        /// </summary>
        [HtmlAttributeName(DictionaryAttributePrefix = LinkAttributesPrefix)]
        public IDictionary<string, string> LinkAttributes { get; set; } = new Dictionary<string, string>();

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var breadcrumbsContext = context.GetContextItem<BreadcrumbsContext>();

            var childContent = await output.GetChildContentAsync();

            var href = HasLinkAttributes ? ResolveHref() : null;

            breadcrumbsContext.AddItem(new BreadcrumbsItem()
            {
                Attributes = output.Attributes.ToAttributesDictionary(),
                Href = href,
                LinkAttributes = LinkAttributes,
                Content = childContent.Snapshot()
            });

            output.SuppressOutput();
        }
    }
}
