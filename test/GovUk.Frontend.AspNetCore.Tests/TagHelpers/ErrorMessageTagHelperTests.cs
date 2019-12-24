using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class ErrorMessageTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-error-message",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-message",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("An error!");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<IHtmlGenerator>();

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new ErrorMessageTagHelper(new DefaultGovUkHtmlGenerator(htmlGenerator.Object));

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("span", output.TagName);
            Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
            Assert.False(output.Attributes.ContainsName("id"));
            Assert.Equal("govuk-error-message", output.Attributes["class"].Value);
            Assert.Equal("<span class=\"govuk-visually-hidden\">Error</span>An error!", output.Content.GetContent());
        }

        [Fact]
        public async Task ProcessAsync_WithVisuallyHiddenTextSpecifiedGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-error-message",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-message",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("An error!");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<IHtmlGenerator>();

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new ErrorMessageTagHelper(new DefaultGovUkHtmlGenerator(htmlGenerator.Object))
            {
                VisuallyHiddenText = "Overriden"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("span", output.TagName);
            Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
            Assert.False(output.Attributes.ContainsName("id"));
            Assert.Equal("govuk-error-message", output.Attributes["class"].Value);
            Assert.Equal("<span class=\"govuk-visually-hidden\">Overriden</span>An error!", output.Content.GetContent());
        }

        [Fact]
        public async Task ProcessAsync_AspForSpecifiedGeneratesExpectedContent()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-error-message",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-message",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<IHtmlGenerator>();
            htmlGenerator
                .Setup(
                    mock => mock.GenerateValidationMessage(
                        /*viewContext: */It.IsAny<ViewContext>(),
                        /*modelExplorer: */It.IsAny<ModelExplorer>(),
                        /*expression: */It.IsAny<string>(),
                        /*message: */It.IsAny<string>(),
                        /*tag: */It.IsAny<string>(),
                        /*htmlAttributes: */It.IsAny<IDictionary<string, object>>()))
                .Returns(() =>
                {
                    var tagBuilder = new TagBuilder("span");
                    tagBuilder.InnerHtml.Append("An error!");
                    return tagBuilder;
                });

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new ErrorMessageTagHelper(new DefaultGovUkHtmlGenerator(htmlGenerator.Object))
            {
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("span", output.TagName);
            Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
            Assert.Equal("govuk-error-message", output.Attributes["class"].Value);
            Assert.Equal("<span class=\"govuk-visually-hidden\">Error</span>An error!", output.Content.GetContent());
        }

        [Fact]
        public async Task ProcessAsync_ContentAndAspForBothSpecifiedThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-error-message",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-message",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("An error!");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<IHtmlGenerator>();

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new ErrorMessageTagHelper(new DefaultGovUkHtmlGenerator(htmlGenerator.Object))
            {
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Cannot specify both content and the 'asp-for' attribute.", ex.Message);
        }

        public class Model
        {
            public string Foo { get; set; }
        }
    }
}
