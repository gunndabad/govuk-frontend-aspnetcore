using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class BackLinkTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_WithContentGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-back-link",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-back-link",
                attributes: new TagHelperAttributeList()
                {
                    { "href", "http://foo.com" }
                },
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });
            output.Content.SetContent("My custom link content");

            var htmlGenerator = new Mock<IHtmlGenerator>();
            htmlGenerator
                .Setup(
                    mock => mock.GenerateActionLink(
                        /*viewContext: */It.IsAny<ViewContext>(),
                        /*linkText: */It.IsAny<string>(),
                        /*actionName: */It.IsAny<string>(),
                        /*controllerName: */It.IsAny<string>(),
                        /*protocol: */It.IsAny<string>(),
                        /*hostname: */It.IsAny<string>(),
                        /*fragment: */It.IsAny<string>(),
                        /*routeValues: */It.IsAny<IDictionary<string, object>>(),
                        /*htmlAttributes: */It.IsAny<object>()))
                .Returns(new TagBuilder("a"));

            var tagHelper = new BackLinkTagHelper(htmlGenerator.Object);

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("a", output.TagName);
            Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
            Assert.Equal("http://foo.com", output.Attributes["href"].Value);
            Assert.Equal("govuk-back-link", output.Attributes["class"].Value);
            Assert.Equal("My custom link content", output.Content.GetContent());
        }

        [Fact]
        public async Task ProcessAsync_WithNoContentGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-back-link",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-back-link",
                attributes: new TagHelperAttributeList()
                {
                    { "href", "http://foo.com" }
                },
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });
            output.Content.SetContent(string.Empty);

            var htmlGenerator = new Mock<IHtmlGenerator>();
            htmlGenerator
                .Setup(
                    mock => mock.GenerateActionLink(
                        /*viewContext: */It.IsAny<ViewContext>(),
                        /*linkText: */It.IsAny<string>(),
                        /*actionName: */It.IsAny<string>(),
                        /*controllerName: */It.IsAny<string>(),
                        /*protocol: */It.IsAny<string>(),
                        /*hostname: */It.IsAny<string>(),
                        /*fragment: */It.IsAny<string>(),
                        /*routeValues: */It.IsAny<IDictionary<string, object>>(),
                        /*htmlAttributes: */It.IsAny<object>()))
                .Returns(new TagBuilder("a"));

            var tagHelper = new BackLinkTagHelper(htmlGenerator.Object);

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("a", output.TagName);
            Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
            Assert.Equal("http://foo.com", output.Attributes["href"].Value);
            Assert.Equal("govuk-back-link", output.Attributes["class"].Value);
            Assert.Equal("Back", output.Content.GetContent());
        }
    }
}
