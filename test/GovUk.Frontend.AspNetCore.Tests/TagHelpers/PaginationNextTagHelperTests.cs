using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class PaginationNextTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_SetsNextOnContext()
        {
            // Arrange
            var paginationContext = new PaginationContext();

            var context = new TagHelperContext(
                tagName: "govuk-pagination-next",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(PaginationContext), paginationContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-pagination-next",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new PaginationNextTagHelper()
            {
                LabelText = "Next page"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("Next page", paginationContext.Next?.LabelText);
        }
    }
}
