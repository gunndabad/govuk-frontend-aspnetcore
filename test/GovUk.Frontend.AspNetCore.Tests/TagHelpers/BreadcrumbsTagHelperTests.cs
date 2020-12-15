using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class BreadcrumbsTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-breadcrumbs",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-breadcrumbs",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var breadcrumbsContext = context.GetContextItem<BreadcrumbsContext>();

                    breadcrumbsContext.AddItem(new BreadcrumbsItem()
                    {
                        Href = "first",
                        Content = new HtmlString("First")
                    });

                    breadcrumbsContext.AddItem(new BreadcrumbsItem()
                    {
                        Href = "second",
                        Content = new HtmlString("Second")
                    });

                    breadcrumbsContext.AddItem(new BreadcrumbsItem()
                    {
                        Content = new HtmlString("Last")
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new BreadcrumbsTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"<div class=""govuk-breadcrumbs"">
<ol class=""govuk-breadcrumbs__list"">
<li class=""govuk-breadcrumbs__list-item""><a class=""govuk-breadcrumbs__link"" href=""first"">First</a></li>
<li class=""govuk-breadcrumbs__list-item""><a class=""govuk-breadcrumbs__link"" href=""second"">Second</a></li>
<li aria-current=""page"" class=""govuk-breadcrumbs__list-item"">Last</li>
</ol>
</div>";

            AssertEx.HtmlEqual(@expectedHtml, output.RenderToString());
        }
    }
}
