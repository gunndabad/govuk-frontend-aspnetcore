using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class SkipLinkTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_WithHrefSpecified_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-skip-link",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-skip-link",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Link content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SkipLinkTagHelper()
            {
                Href = "#main"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<a class=""govuk-skip-link"" href=""#main"">Link content</a>";

            AssertEx.HtmlEqual(expectedHtml, output.RenderToString());
        }

        [Fact]
        public async Task ProcessAsync_WithNoHrefSpecified_UsesDefaultHref()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-skip-link",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-skip-link",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Link content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SkipLinkTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var element = output.RenderToElement();

            Assert.Equal("#content", element.GetAttribute("href"));
        }
    }
}
