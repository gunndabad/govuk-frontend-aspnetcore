using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class HintTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_WithContentGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-hint",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-hint",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Hint text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new HintTagHelper(new DefaultGovUkHtmlGenerator());

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal("<span class=\"govuk-hint\">Hint text</span>", html);
        }
    }
}
