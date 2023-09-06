using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class SummaryCardActionsTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddsAttributesToContext()
        {
            // Arrange
            var summaryCardContext = new SummaryCardContext();

            var context = new TagHelperContext(
                tagName: "govuk-summary-card-actions",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(SummaryCardContext), summaryCardContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-summary-card-actions",
                attributes: new TagHelperAttributeList()
                {
                    { "class", "additional-class" }
                },
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SummaryCardActionsTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Collection(
                summaryCardContext.ActionsAttributes,
                kvp =>
                {
                    Assert.Equal("class", kvp.Key);
                    Assert.Equal("additional-class", kvp.Value);
                });
        }
    }
}
