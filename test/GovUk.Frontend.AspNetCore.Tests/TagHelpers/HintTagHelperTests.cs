using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
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

            var tagHelper = new HintTagHelper(new DefaultGovUkHtmlGenerator(Mock.Of<IHtmlGenerator>()));

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("span", output.TagName);
            Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
            Assert.Equal("govuk-hint", output.Attributes["class"].Value);
            Assert.Equal("Hint text", output.Content.GetContent());
        }
    }
}
