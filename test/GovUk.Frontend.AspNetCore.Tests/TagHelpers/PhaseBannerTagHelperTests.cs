using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
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
                    var pbContext = (PhaseBannerContext)context.Items[typeof(PhaseBannerContext)];
                    pbContext.TrySetTag(attributes: null, content: new HtmlString("Alpha"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Phase message");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new PhaseBannerTagHelper(new ComponentGenerator());

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
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

            var tagHelper = new PhaseBannerTagHelper(new ComponentGenerator());

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("You must specify a <govuk-phase-banner-tag> child element.", ex.Message);
        }
    }

    public class PhaseBannerTagTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddsToContext()
        {
            // Arrange
            var pbContext = new PhaseBannerContext();

            var context = new TagHelperContext(
                tagName: "govuk-phase-banner-tag",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(PhaseBannerContext), pbContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-phase-banner-tag",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Legend message");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new PhaseBannerTagTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("Legend message", pbContext.Tag?.content.RenderToString());
        }

        [Fact]
        public async Task ProcessAsync_TagAlreadySpecifiedThrowsInvalidOperationException()
        {
            // Arrange
            var pbContext = new PhaseBannerContext();
            pbContext.TrySetTag(attributes: null, content: new HtmlString("Existing tag"));

            var context = new TagHelperContext(
                tagName: "govuk-phase-banner-tag",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(PhaseBannerContext), pbContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-phase-banner-tag",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Legend message");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new PhaseBannerTagTagHelper();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Cannot render <govuk-phase-banner-tag> here.", ex.Message);
        }
    }
}
