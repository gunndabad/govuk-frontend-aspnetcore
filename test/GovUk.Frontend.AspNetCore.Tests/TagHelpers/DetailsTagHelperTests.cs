using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class DetailsTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-details",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-details",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var detailsContext = (DetailsContext)context.Items[DetailsContext.ContextName];

                    var summary = new HtmlString("The summary");
                    detailsContext.SetSummary(attributes: null, summary);

                    var text = new HtmlString("The text");
                    detailsContext.SetText(attributes: null, text);

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new DetailsTagHelper(new DefaultGovUkHtmlGenerator());

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<details class=\"govuk-details\" data-module=\"govuk-details\">" +
                "<summary class=\"govuk-details__summary\">" +
                "<span class=\"govuk-details__summary-text\">The summary</span>" +
                "</summary>" +
                "<div class=\"govuk-details__text\">The text</div>" +
                "</details>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_WithIdSpecifiedAddsIdAttributeToOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-details",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-details",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var detailsContext = (DetailsContext)context.Items[DetailsContext.ContextName];

                    var summary = new HtmlString("The summary");
                    detailsContext.SetSummary(attributes: null, summary);

                    var text = new HtmlString("The text");
                    detailsContext.SetText(attributes: null, text);

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new DetailsTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Id = "my-id"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("my-id", output.Attributes["id"].Value);
        }

        [Fact]
        public async Task ProcessAsync_WithOpenSpecifiedAddsOpenAttributeToOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-details",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-details",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var detailsContext = (DetailsContext)context.Items[DetailsContext.ContextName];

                    var summary = new HtmlString("The summary");
                    detailsContext.SetSummary(attributes: null, summary);

                    var text = new HtmlString("The text");
                    detailsContext.SetText(attributes: null, text);

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new DetailsTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Open = true
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("true", output.Attributes["open"].Value);
        }
    }

    public class DetailsSummaryTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_SetsContentOnContext()
        {
            // Arrange
            var detailsContext = new DetailsContext();

            var context = new TagHelperContext(
                tagName: "govuk-details-summary",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { DetailsContext.ContextName, detailsContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-details-summary",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("The summary");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new DetailsSummaryTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("The summary", detailsContext.Summary?.content.AsString());
        }
    }

    public class DetailsTextTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_SetsContentOnContext()
        {
            // Arrange
            var detailsContext = new DetailsContext();
            detailsContext.SetSummary(attributes: null, new HtmlString("The summary"));

            var context = new TagHelperContext(
                tagName: "govuk-details-text",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { DetailsContext.ContextName, detailsContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-details-text",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("The text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new DetailsTextTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("The text", detailsContext.Text?.content.AsString());
        }
    }

    public class DetailsContextTests
    {
        [Fact]
        public void SetSummary_StageAtTextThrowsInvalidOperationException()
        {
            // Arrange
            var ctx = new DetailsContext();
            ctx.SetSummary(attributes: null, new HtmlString("Summary"));
            ctx.SetText(attributes: null, new HtmlString("Text"));

            // Act & Assert
            Assert.Equal(DetailsRenderStage.Text, ctx.RenderStage);
            var ex = Assert.Throws<InvalidOperationException>(() => ctx.SetSummary(attributes: null, new HtmlString("Summary")));
            Assert.Equal("Cannot render <govuk-details-summary> here.", ex.Message);
        }

        [Fact]
        public void SetSummary_StageAtSummaryThrowsInvalidOperationException()
        {
            // Arrange
            var ctx = new DetailsContext();
            ctx.SetSummary(attributes: null, new HtmlString("Summary"));

            // Act & Assert
            Assert.Equal(DetailsRenderStage.Summary, ctx.RenderStage);
            var ex = Assert.Throws<InvalidOperationException>(() => ctx.SetSummary(attributes: null, new HtmlString("Summary")));
            Assert.Equal("Cannot render <govuk-details-summary> here.", ex.Message);
        }

        [Fact]
        public void ThrowIfStagesIncomplete_StageAtNoneThrowsInvalidOperationException()
        {
            // Arrange
            var ctx = new DetailsContext();

            // Act & Assert
            Assert.Equal(DetailsRenderStage.None, ctx.RenderStage);
            var ex = Assert.Throws<InvalidOperationException>(() => ctx.ThrowIfStagesIncomplete());
            Assert.Equal("Missing one or more child elements.", ex.Message);
        }

        [Fact]
        public void ThrowIfStagesIncomplete_StageAtSummaryThrowsInvalidOperationException()
        {
            // Arrange
            var ctx = new DetailsContext();
            ctx.SetSummary(attributes: null, new HtmlString("Summary"));

            // Act & Assert
            Assert.Equal(DetailsRenderStage.Summary, ctx.RenderStage);
            var ex = Assert.Throws<InvalidOperationException>(() => ctx.ThrowIfStagesIncomplete());
            Assert.Equal("Missing one or more child elements.", ex.Message);
        }

        [Fact]
        public void ThrowIfStagesIncomplete_StageAtTextDoesNotThrow()
        {
            // Arrange
            var ctx = new DetailsContext();
            ctx.SetSummary(attributes: null, new HtmlString("Summary"));
            ctx.SetText(attributes: null, new HtmlString("Text"));

            // Act & Assert
            Assert.Equal(DetailsRenderStage.Text, ctx.RenderStage);
            ctx.ThrowIfStagesIncomplete();
        }
    }
}
