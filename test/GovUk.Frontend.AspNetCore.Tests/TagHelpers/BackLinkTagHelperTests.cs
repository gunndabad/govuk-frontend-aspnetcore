using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class BackLinkTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_WithContent_GeneratesExpectedOutput()
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

            var tagHelper = new BackLinkTagHelper(Mock.Of<IUrlHelperFactory>())
            {
                Href = "http://foo.com"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<a class=""govuk-back-link"" href=""http://foo.com"">My custom link content</a>";

            AssertEx.HtmlEqual(@expectedHtml, output.RenderToString());
        }

        [Fact]
        public async Task ProcessAsync_WithNoContent_GeneratesExpectedOutput()
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

            var tagHelper = new BackLinkTagHelper(Mock.Of<IUrlHelperFactory>())
            {
                Href = "http://foo.com"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<a class=""govuk-back-link"" href=""http://foo.com"">Back</a>";

            AssertEx.HtmlEqual(@expectedHtml, output.RenderToString());
        }
    }
}
