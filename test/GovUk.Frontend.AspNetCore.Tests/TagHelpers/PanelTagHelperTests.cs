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
    public class PanelTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-panel",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-panel",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var panelContext = (PanelContext)context.Items[typeof(PanelContext)];
                    panelContext.TrySetTitle(new HtmlString("Title"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetHtmlContent("Body");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new PanelTagHelper(new ComponentGenerator())
            {
                HeadingLevel = 3
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal(
                "<div class=\"govuk-panel--confirmation govuk-panel\"><h3 class=\"govuk-panel__title\">Title</h3><div class=\"govuk-panel__body\">Body</div></div>",
                output.RenderToString());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(7)]
        public async Task ProcessAsync_InvalidheadingLevelThrowsInvalidOperationException(int level)
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-panel",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-panel",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var panelContext = (PanelContext)context.Items[typeof(PanelContext)];
                    panelContext.TrySetTitle(new HtmlString("Title"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetHtmlContent("Body");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new PanelTagHelper(new ComponentGenerator())
            {
                HeadingLevel = level
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("The 'heading-level' attribute must be between 1 and 6.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_NoTitleThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-panel",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-panel",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var panelContext = (PanelContext)context.Items[typeof(PanelContext)];

                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetHtmlContent("Body");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new PanelTagHelper(new ComponentGenerator());

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Missing <govuk-panel-title> element.", ex.Message);
        }
    }

    public class PanelTitleTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddsTitleToContext()
        {
            // Arrange
            var panelContext = new PanelContext();

            var context = new TagHelperContext(
                tagName: "govuk-panel-title",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(PanelContext), panelContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-panel-title",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetHtmlContent("Title");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new PanelTitleTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.NotNull(panelContext.Title);
            Assert.Equal("Title", panelContext.Title.RenderToString());
        }

        [Fact]
        public async Task ProcessAsync_ItemAlreadyHasTitleThrowsInvalidOperationException()
        {
            // Arrange
            var panelContext = new PanelContext();
            panelContext.TrySetTitle(content: new HtmlString("Existing title"));

            var context = new TagHelperContext(
                tagName: "govuk-panel-title",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(PanelContext), panelContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-panel-title",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetHtmlContent("Title");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new PanelTitleTagHelper();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Cannot render <govuk-panel-title> here.", ex.Message);
        }
    }
}
