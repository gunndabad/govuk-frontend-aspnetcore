using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class HintTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
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

            var tagHelper = new HintTagHelper(new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>()));

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal("<span class=\"govuk-hint\">Hint text</span>", html);
        }

        [Fact]
        public async Task ProcessAsync_WithIdSpecifiedIncludesIdInOutput()
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

            var tagHelper = new HintTagHelper(new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>()))
            {
                Id = "some-id"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("some-id", output.Attributes["id"].Value);
        }
    }
}
