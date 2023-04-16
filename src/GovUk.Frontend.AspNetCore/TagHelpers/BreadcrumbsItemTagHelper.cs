using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents an item in a GDS breadcrumbs component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = BreadcrumbsTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = BreadcrumbsTagHelper.TagName)]
    //[OutputElementHint(ComponentGenerator.BreadcrumbsItemElement)]  // Omitted since it produces intellisense warnings
    public class BreadcrumbsItemTagHelper : TagHelper
    {
        internal const string TagName = "govuk-breadcrumbs-item";
        internal const string ShortTagName = "breadcrumbs-item";

        private const string LinkAttributesPrefix = "link-";

        /// <summary>
        /// Additional attributes for the generated <c>a</c> element.
        /// </summary>
        [HtmlAttributeName(DictionaryAttributePrefix = LinkAttributesPrefix)]
        public IDictionary<string, string?> LinkAttributes { get; set; } = new Dictionary<string, string?>();

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var breadcrumbsContext = context.GetContextItem<BreadcrumbsContext>();

            var childContent = await output.GetChildContentAsync();

            string? href = null;

            if (output.Attributes.TryGetAttribute("href", out var hrefAttribute))
            {
                href = hrefAttribute.Value.ToString();
                output.Attributes.Remove(hrefAttribute);
            }

            breadcrumbsContext.AddItem(new BreadcrumbsItem()
            {
                Attributes = output.Attributes.ToAttributeDictionary(),
                Href = href,
                LinkAttributes = LinkAttributes.ToAttributeDictionary(),
                Content = childContent.Snapshot()
            });

            output.SuppressOutput();
        }
    }
}
