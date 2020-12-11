using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Mvc.Routing;
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
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("My custom link content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });
            output.Content.SetContent("My custom link content");

            var tagHelper = new BackLinkTagHelper(
                new DefaultGovUkHtmlGenerator(),
                Mock.Of<IUrlHelperFactory>())
            {
                Href = "http://foo.com"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            Assert.Equal("<a class=\"govuk-back-link\" href=\"http://foo.com\">My custom link content</a>", html);
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
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });
            output.TagMode = TagMode.SelfClosing;

            var tagHelper = new BackLinkTagHelper(
                new DefaultGovUkHtmlGenerator(),
                Mock.Of<IUrlHelperFactory>())
            {
                Href = "http://foo.com"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            Assert.Equal("<a class=\"govuk-back-link\" href=\"http://foo.com\">Back</a>", html);
        }
    }
}
