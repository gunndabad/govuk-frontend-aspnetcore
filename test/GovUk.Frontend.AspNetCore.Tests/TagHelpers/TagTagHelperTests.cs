using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class TagTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_WithContentGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-tag",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-tag",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("A tag");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TagTagHelper(new DefaultGovUkHtmlGenerator());

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            Assert.Equal("<strong class=\"govuk-tag\">A tag</strong>", html);
        }
    }
}
