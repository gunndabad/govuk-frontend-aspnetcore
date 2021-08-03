using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class InputSuffixTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_SetsSuffixOnContext()
        {
            // Arrange
            var inputContext = new InputContext();

            var context = new TagHelperContext(
                tagName: "govuk-input-suffix",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(InputContext), inputContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-input-suffix",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Suffix"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new InputSuffixTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.True(inputContext.Suffix.HasValue);
            Assert.Equal("Suffix", inputContext.Suffix.Value.Content.RenderToString());
        }
    }
}
