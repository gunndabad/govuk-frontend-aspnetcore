using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class PhaseBannerTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-phase-banner",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-phase-banner",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Phase message");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new PhaseBannerTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Tag = "Alpha"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<div class=\"govuk-phase-banner\">" +
                "<p class=\"govuk-phase-banner__content\">" +
                "<strong class=\"govuk-phase-banner__content__tag govuk-tag\">Alpha</strong>" +
                "<span class=\"govuk-phase-banner__text\">Phase message</span>" +
                "</p>" +
                "</div>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_MissingIconFallbackTextThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-phase-banner",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-phase-banner",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Phase message");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new PhaseBannerTagHelper(new DefaultGovUkHtmlGenerator());

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("You must specify a value for the 'tag' attribute.", ex.Message);
        }
    }
}
