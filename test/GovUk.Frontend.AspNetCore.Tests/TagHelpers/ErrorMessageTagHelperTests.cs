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

            var htmlHelper = new Mock<IHtmlHelper>();

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new ErrorMessageTagHelper(new DefaultGovUkHtmlGenerator(), new DefaultModelHelper());

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<span class=\"govuk-error-message\">" +
                "<span class=\"govuk-visually-hidden\">Error</span>" +
                "An error!" +
                "</span>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_WithIdSpecifiedGeneratesExpectedOutput()
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

            var htmlHelper = new Mock<IHtmlHelper>();

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new ErrorMessageTagHelper(new DefaultGovUkHtmlGenerator(), new DefaultModelHelper())
            {
                Id = "some-id"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("some-id", output.Attributes["id"].Value);
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

            var htmlHelper = new Mock<IHtmlHelper>();

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new ErrorMessageTagHelper(new DefaultGovUkHtmlGenerator(), new DefaultModelHelper())
            {
                VisuallyHiddenText = "Overriden"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<span class=\"govuk-error-message\">" +
                "<span class=\"govuk-visually-hidden\">Overriden</span>" +
                "An error!" +
                "</span>",
                html);
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
            output.TagMode = TagMode.SelfClosing;

            var modelHelperMock = new Mock<IModelHelper>();

            modelHelperMock
                .Setup(mock => mock.GetValidationMessage(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("An error!");

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new ErrorMessageTagHelper(new DefaultGovUkHtmlGenerator(), modelHelperMock.Object)
            {
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<span class=\"govuk-error-message\">" +
                "<span class=\"govuk-visually-hidden\">Error</span>" +
                "An error!" +
                "</span>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_AspForSpecifiedButNoErrorGeneratesNoOutput()
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
            output.TagMode = TagMode.SelfClosing;

            var modelHelperMock = new Mock<IModelHelper>();

            modelHelperMock
                .Setup(mock => mock.GetValidationMessage(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns((string)null);

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new ErrorMessageTagHelper(new DefaultGovUkHtmlGenerator(), modelHelperMock.Object)
            {
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Null(output.TagName);
        }

        public class Model
        {
            public string Foo { get; set; }
        }
    }
}
