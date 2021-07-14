using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class InsetTextTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_WithContentGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-inset-text",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-inset-text",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Inset text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new InsetTextTagHelper(new ComponentGenerator())
            {
                Id = "my-id"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"<div class=""govuk-inset-text"" id=""my-id"">Inset text</div>";

            AssertEx.HtmlEqual(expectedHtml, output.RenderToString());
        }
    }
}
