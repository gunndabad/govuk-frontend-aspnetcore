using System;
using System.Collections.Generic;
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

            var tagHelper = new WarningTextTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IconFallbackText = "Danger"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<div class=\"govuk-warning-text\">" +
                "<span aria-hidden=\"true\" class=\"govuk-warning-text__icon\">!</span>" +
                "<strong class=\"govuk-warning-text__text\">" +
                "<span class=\"govuk-warning-text__assistive\">Danger</span>" +
                "Warning message" +
                "</strong>" +
                "</div>",
                html);
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

            var tagHelper = new WarningTextTagHelper(new DefaultGovUkHtmlGenerator());

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("You must specify a value for the 'icon-fallback-text' attribute.", ex.Message);
        }
    }
}
