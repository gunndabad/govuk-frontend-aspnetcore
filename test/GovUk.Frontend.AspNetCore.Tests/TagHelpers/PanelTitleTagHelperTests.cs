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
    public class PanelTitleTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_SetsTitleOnContext()
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
                    tagHelperContent.SetContent("The title");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new PanelTitleTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("The title", panelContext.Title?.ToHtmlString());
        }

        [Fact]
        public async Task ProcessAsync_ParentAlreadyHasTitle_ThrowsInvalidOperationException()
        {
            // Arrange
            var panelContext = new PanelContext();
            panelContext.SetTitle(new HtmlString("The title"));

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
                    tagHelperContent.SetContent("The title");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new PanelTitleTagHelper();

            // Act
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Only one <govuk-panel-title> element is permitted within each <govuk-panel>.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_ParentAlreadyHasBody_ThrowsInvalidOperationException()
        {
            // Arrange
            var panelContext = new PanelContext();
            panelContext.SetBody(new HtmlString("The body"));

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
                    tagHelperContent.SetContent("The title");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new PanelTitleTagHelper();

            // Act
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-panel-title> must be specified before <govuk-panel-body>.", ex.Message);
        }
    }
}
