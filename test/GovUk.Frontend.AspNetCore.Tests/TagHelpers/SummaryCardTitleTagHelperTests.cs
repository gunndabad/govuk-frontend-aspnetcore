using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class SummaryCardTitleTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_SetsTitleOnContext()
        {
            // Arrange
            var summaryCardContext = new SummaryCardContext();

            var context = new TagHelperContext(
                tagName: "govuk-summary-card-title",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(SummaryCardContext), summaryCardContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-summary-card-title",
                attributes: new TagHelperAttributeList()
                {
                    { "class", "additional-class" }
                },
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Title");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SummaryCardTitleTagHelper()
            {
                HeadingLevel = 3
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("Title", summaryCardContext.Title?.Content?.ToHtmlString());
            Assert.Equal(3, summaryCardContext.Title?.HeadingLevel);
        }
    }
}
