using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class CharacterCountTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_NoMaxLengthOrMaxWordsThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-character-count",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-character-count",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (TextAreaBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(isPageHeading: false, content: new HtmlString("The label"));
                    formGroupContext.TrySetElementContent(new HtmlString("The content"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CharacterCountTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Id = "my-id",
                Name = "my-name"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("One of the 'max-length' and 'max-words' attributes must be specified.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_BothMaxLengthAndMaxWordsThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-character-count",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-character-count",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (TextAreaBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(isPageHeading: false, content: new HtmlString("The label"));
                    formGroupContext.TrySetElementContent(new HtmlString("The content"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CharacterCountTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Id = "my-id",
                Name = "my-name",
                MaxWords = 10,
                MaxLength = 200
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("The 'max-length' and 'max-words' attributes are mutually exclusive.", ex.Message);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        public async Task ProcessAsync_ThresholdInvalidThrowsInvalidOperationException(decimal threshold)
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-character-count",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-character-count",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (TextAreaBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(isPageHeading: false, content: new HtmlString("The label"));
                    formGroupContext.TrySetElementContent(new HtmlString("The content"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CharacterCountTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Id = "my-id",
                Name = "my-name",
                MaxWords = 10,
                Threshold = threshold
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("The 'threshold' attribute is invalid.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_WithMaxWordsGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-character-count",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-character-count",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (TextAreaBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(isPageHeading: false, content: new HtmlString("The label"));
                    formGroupContext.TrySetElementContent(new HtmlString("The content"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CharacterCountTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Id = "my-id",
                Name = "my-name",
                MaxWords = 10,
                Threshold = 90
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            var node = HtmlNode.CreateNode(html);

            Assert.Equal("govuk-character-count", node.Attributes["class"].Value);
            Assert.Equal("govuk-character-count", node.Attributes["data-module"].Value);
            Assert.Equal("10", node.Attributes["data-maxwords"].Value);
            Assert.Equal("90", node.Attributes["data-threshold"].Value);
            Assert.DoesNotContain(node.Attributes, a => a.Name == "data-maxlength");

            var textarea = node.ChildNodes.FindFirst("textarea");
            Assert.Contains("govuk-js-character-count", textarea.GetCssClasses());

            var hint = node.SelectSingleNode("//span[contains(@class, 'govuk-hint')]");
            Assert.Equal("my-id-info", hint.Id);
            Assert.Contains("govuk-character-count__message", hint.GetCssClasses());
            Assert.Equal("polite", hint.Attributes["aria-live"].Value);
            Assert.Equal("You can enter up to 10 words", hint.InnerText);
        }

        [Fact]
        public async Task ProcessAsync_WithMaxLengthGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-character-count",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-character-count",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (TextAreaBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(isPageHeading: false, content: new HtmlString("The label"));
                    formGroupContext.TrySetElementContent(new HtmlString("The content"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CharacterCountTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Id = "my-id",
                Name = "my-name",
                MaxLength = 200,
                Threshold = 90
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            var node = HtmlNode.CreateNode(html);

            Assert.Equal("govuk-character-count", node.Attributes["class"].Value);
            Assert.Equal("govuk-character-count", node.Attributes["data-module"].Value);
            Assert.Equal("200", node.Attributes["data-maxlength"].Value);
            Assert.Equal("90", node.Attributes["data-threshold"].Value);
            Assert.DoesNotContain(node.Attributes, a => a.Name == "data-maxwords");

            var textarea = node.ChildNodes.FindFirst("textarea");
            Assert.Contains("govuk-js-character-count", textarea.GetCssClasses());

            var hint = node.SelectSingleNode("//span[contains(@class, 'govuk-hint')]");
            Assert.Equal("my-id-info", hint.Id);
            Assert.Contains("govuk-character-count__message", hint.GetCssClasses());
            Assert.Equal("polite", hint.Attributes["aria-live"].Value);
            Assert.Equal("You can enter up to 200 characters", hint.InnerText);
        }
    }
}
