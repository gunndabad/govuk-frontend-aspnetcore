using GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class RazorGenerator
    {
        public string GenerateBreadcrumbs(Breadcrumbs options)
        {
            var breadcrumbs = new TagBuilder("govuk-breadcrumbs");

            breadcrumbs.AddCssClass(options.Classes);
            breadcrumbs.MergeAttributes(options.Attributes);

            if (options.CollapseOnMobile.HasValue)
            {
                breadcrumbs.Attributes.Add("collapse-on-mobile", options.CollapseOnMobile.Value ? "true" : "false");
            }

            foreach (var item in options.Items.OrEmpty())
            {
                var breadcrumbsItem = new TagBuilder("govuk-breadcrumbs-item");
                MergeAttributesWithPrefix(breadcrumbsItem, prefix: "link", item.Attributes);

                if (item.Href != null)
                {
                    breadcrumbsItem.Attributes.Add("href", item.Href);
                }

                breadcrumbsItem.InnerHtml.AppendHtml(item.GetHtmlContent());

                breadcrumbs.InnerHtml.AppendHtml(breadcrumbsItem);
            }

            return breadcrumbs.RenderToString();
        }
    }
}
