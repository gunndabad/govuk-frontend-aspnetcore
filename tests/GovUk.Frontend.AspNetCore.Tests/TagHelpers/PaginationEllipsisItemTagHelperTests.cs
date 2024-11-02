using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class PaginationEllipsisItemTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_AddsItemToContextItems()
    {
        // Arrange
        var paginationContext = new PaginationContext();

        var context = new TagHelperContext(
            tagName: "govuk-pagination-ellipsis",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>() { { typeof(PaginationContext), paginationContext } },
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-pagination-ellipsis",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new PaginationEllipsisItemTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(paginationContext.Items, item => Assert.IsType<PaginationItemEllipsis>(item));
    }
}
