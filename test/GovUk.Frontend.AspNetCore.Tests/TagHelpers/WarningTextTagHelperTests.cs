using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class WarningTextTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
            tagName: "govuk-warning-text",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-warning-text",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Warning message");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new WarningTextTagHelper()
            {
                IconFallbackText = "Danger"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("div", output.TagName);
            Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
            Assert.Equal("govuk-warning-text", output.Attributes["class"].Value);
            Assert.Equal(
                "<span aria-hidden=\"true\" class=\"govuk-warning-text__icon\">!</span>" +
                "<strong class=\"govuk-warning-text__text\">" +
                "<span class=\"govuk-warning-text__assistive\">Danger</span>" +
                "Warning message" +
                "</strong>",
                output.Content.GetContent());
        }

        [Fact]
        public async Task ProcessAsync_MissingIconFallbackTextThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
            tagName: "govuk-warning-text",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-warning-text",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Warning message");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new WarningTextTagHelper();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("You must specify a value for the 'icon-fallback-text' attribute.", ex.Message);
        }
    }
}
