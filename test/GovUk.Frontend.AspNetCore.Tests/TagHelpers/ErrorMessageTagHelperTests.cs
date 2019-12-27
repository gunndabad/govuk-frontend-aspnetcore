using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
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

            var tagHelper = new ErrorMessageTagHelper(new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>()), htmlHelper.Object);

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

            var tagHelper = new ErrorMessageTagHelper(new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>()), htmlHelper.Object)
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

            var tagHelper = new ErrorMessageTagHelper(new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>()), htmlHelper.Object)
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

            var htmlHelper = new Mock<IHtmlHelper>();
            htmlHelper
                .Setup(
                    mock => mock.ValidationMessage(
                        /*expression: */It.IsAny<string>(),
                        /*message: */It.IsAny<string>(),
                        /*htmlAttributes: */It.IsAny<IDictionary<string, object>>(),
                        /*tag: */It.IsAny<string>()))
                .Returns(new StringHtmlContent("An error!"));

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new ErrorMessageTagHelper(new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>()), htmlHelper.Object)
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

        public class Model
        {
            public string Foo { get; set; }
        }
    }
}
