using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class ErrorSummaryDescriptionTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddsDescriptionToContext()
        {
            // Arrange
            var errorSummaryContext = new ErrorSummaryContext();

            var context = new TagHelperContext(
                tagName: "govuk-error-summary-description",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(ErrorSummaryContext), errorSummaryContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-summary-description",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Description content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ErrorSummaryDescriptionTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("Description content", errorSummaryContext?.Description?.Content?.ToHtmlString());
        }
    }
}
